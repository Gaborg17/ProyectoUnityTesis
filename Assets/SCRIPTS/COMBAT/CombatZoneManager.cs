using UnityEngine;

public class CombatZoneManager : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine[] enemies;
    [SerializeField] private AllyStateMachine[] alies;

    [SerializeField]private PlayerStateMachine player;

    private void Start()
    {
        AssignCZM();
    }
    public void AssignCZM()
    {
        foreach(EnemyStateMachine enemy in enemies)
        {
            enemy.CombatManager = this;
        }
    }


    public void MatchUp()
    {
        for (int i = 0;  i < enemies.Length; i++)
        {
            enemies[i].InCombat = true;
            alies[i].InCombat = true;
            enemies[i].AssignedTarget = alies[i].transform;
            alies[i].AssignedEnemy = enemies[i].transform;
        }
    }
}
