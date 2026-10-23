using UnityEngine;

public class DeathWatcher : MonoBehaviour
{
    [HideInInspector] public CombatZoneManager manager;
    [HideInInspector] public EnemyStateMachine enemy;
    [HideInInspector] public AllyStateMachine ally;

    private bool _fired;

    private void OnEnable() => _fired = false;

    private void OnDisable()
    {
        if (gameObject.scene.isLoaded) FireIfDead();
    }

    private void OnDestroy() => FireIfDead();

    private void FireIfDead()
    {
        if (_fired || manager == null) return;

        if (enemy != null && !enemy.IsAlive) { _fired = true; manager.OnEnemyDied(enemy); }
        else if (ally != null && !ally.IsAlive) { _fired = true; manager.OnAllyDied(ally); }
    }
}
