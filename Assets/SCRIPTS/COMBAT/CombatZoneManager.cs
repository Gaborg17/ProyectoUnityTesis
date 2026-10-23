using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatZoneManager : MonoBehaviour
{
    [Header("Zone")]
    [SerializeField] private EnemyStateMachine[] enemies;
    [SerializeField] private PlayerStateMachine player;
    [SerializeField] private string allyTag = "Ally";

    [Header("Distribution")]
    [SerializeField] private int maxEnemiesOnPlayer = 3;
    [SerializeField] private int minEnemiesPerAlly = 2;
    [SerializeField] private int maxEnemiesPerAlly = 3;

    // Assignment state
    private readonly List<EnemyStateMachine> unassigned = new();
    private readonly List<EnemyStateMachine> onPlayer = new();
    private readonly Dictionary<AllyStateMachine, List<EnemyStateMachine>> onAlly = new();

    // Persistent ally roster — allies never leave mid-combat
    private readonly HashSet<AllyStateMachine> zoneAllies = new();

    private bool isEngaged;

    private void Awake()
    {
        if (player == null) player = FindAnyObjectByType<PlayerStateMachine>();

        GetComponent<Collider>().isTrigger = true;
        if (!TryGetComponent<Rigidbody>(out _))
        {
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private void Start()
    {
        foreach (var e in enemies)
        {
            if (e == null) continue;
            e.CombatManager = this;
            AttachWatcher(e.gameObject, e, null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(allyTag)) return;
        if (other.TryGetComponent<AllyStateMachine>(out var ally) && ally.IsAlive && zoneAllies.Add(ally))
            AttachWatcher(ally.gameObject, null, ally);
    }

    private void OnTriggerExit(Collider other)
    {
        // Only the player leaving ends combat. Allies never leave mid-combat.
        if (player != null && other.transform.IsChildOf(player.transform) && isEngaged)
            Disengage();
    }

    // ── Public API ─────────────────────────────────────────────

    // Called by any enemy that detects the player.
    public void MatchUp()
    {
        if (isEngaged) return;
        isEngaged = true;
        ResetAssignments();

        // Player's share first
        foreach (var e in enemies)
        {
            if (e == null || !e.IsAlive) continue;
            if (player != null && onPlayer.Count < maxEnemiesOnPlayer) onPlayer.Add(e);
            else unassigned.Add(e);
        }

        // Each living ally gets its minimum
        foreach (var ally in zoneAllies)
        {
            if (ally == null || !ally.IsAlive) continue;
            var list = new List<EnemyStateMachine>();
            onAlly[ally] = list;
            for (int i = 0; i < minEnemiesPerAlly; i++)
            {
                var e = PopPool();
                if (e == null) break;
                list.Add(e);
            }
        }

        DistributeExtras();
        ApplyAssignments();
    }

    public void Disengage()
    {
        isEngaged = false;
        foreach (var e in enemies)
            if (e != null) { e.InCombat = false; e.AssignedTarget = null; }
        foreach (var a in onAlly.Keys)
            if (a != null) { a.InCombat = false; a.AssignedEnemy = null; }
        ResetAssignments();
        // zoneAllies is NOT cleared — allies may still be inside the trigger
        // and OnTriggerEnter won't re-fire for them.
    }

    // Called by DeathWatcher.
    public void OnEnemyDied(EnemyStateMachine enemy)
    {
        if (!isEngaged) return;

        bool wasOnPlayer = onPlayer.Remove(enemy);
        AllyStateMachine owner = null;
        foreach (var kv in onAlly)
            if (kv.Value.Remove(enemy)) { owner = kv.Key; break; }
        unassigned.Remove(enemy);

        // Whoever lost their target gets priority from the pool
        if (wasOnPlayer && onPlayer.Count < maxEnemiesOnPlayer)
        {
            var e = PopPool();
            if (e != null) onPlayer.Add(e);
        }
        else if (owner != null && onAlly.TryGetValue(owner, out var list) && list.Count < maxEnemiesPerAlly)
        {
            var e = PopPool();
            if (e != null) list.Add(e);
        }

        DistributeExtras();
        ApplyAssignments();
    }

    // Called by DeathWatcher.
    public void OnAllyDied(AllyStateMachine ally)
    {
        zoneAllies.Remove(ally);
        if (!isEngaged) return;
        if (onAlly.TryGetValue(ally, out var list))
        {
            unassigned.AddRange(list);
            onAlly.Remove(ally);
        }
        DistributeExtras();
        ApplyAssignments();
    }

    // ── Internal ──────────────────────────────────────────────

    private void ResetAssignments()
    {
        unassigned.Clear();
        onPlayer.Clear();
        onAlly.Clear();
    }

    private void DistributeExtras()
    {
        bool changed = true;
        while (changed && unassigned.Count > 0)
        {
            changed = false;
            foreach (var kv in onAlly)
            {
                if (unassigned.Count == 0) break;
                if (kv.Value.Count >= maxEnemiesPerAlly) continue;
                var e = PopPool();
                if (e == null) break;
                kv.Value.Add(e);
                changed = true;
            }
        }
    }

    private EnemyStateMachine PopPool()
    {
        while (unassigned.Count > 0)
        {
            var e = unassigned[0];
            unassigned.RemoveAt(0);
            if (e != null && e.IsAlive) return e;
        }
        return null;
    }

    private void ApplyAssignments()
    {
        foreach (var e in enemies)
        {
            if (e == null || !e.IsAlive) continue;
            e.AssignedTarget = TargetForEnemy(e);
            e.InCombat = e.AssignedTarget != null;
        }
        foreach (var a in onAlly.Keys)
        {
            if (a == null || !a.IsAlive) continue;
            a.AssignedEnemy = NearestForAlly(a);
            a.InCombat = true;
        }
    }

    private Transform TargetForEnemy(EnemyStateMachine enemy)
    {
        if (onPlayer.Contains(enemy)) return player != null ? player.transform : null;
        foreach (var kv in onAlly)
            if (kv.Value.Contains(enemy)) return kv.Key.transform;
        return null;
    }

    private Transform NearestForAlly(AllyStateMachine ally)
    {
        if (ally == null || !onAlly.TryGetValue(ally, out var list)) return null;

        // Keep current target if still valid and in the group
        if (ally.AssignedEnemy != null)
            foreach (var e in list)
                if (e != null && e.IsAlive && e.transform == ally.AssignedEnemy)
                    return ally.AssignedEnemy;

        EnemyStateMachine best = null;
        float bestSq = float.MaxValue;
        foreach (var e in list)
        {
            if (e == null || !e.IsAlive) continue;
            float d = (e.transform.position - ally.transform.position).sqrMagnitude;
            if (d < bestSq) { bestSq = d; best = e; }
        }
        return best != null ? best.transform : null;
    }

    private void AttachWatcher(GameObject go, EnemyStateMachine enemy, AllyStateMachine ally)
    {
        if (!go.TryGetComponent<DeathWatcher>(out var w))
            w = go.AddComponent<DeathWatcher>();
        w.manager = this;
        if (enemy != null) w.enemy = enemy;
        if (ally != null) w.ally = ally;
    }
}
