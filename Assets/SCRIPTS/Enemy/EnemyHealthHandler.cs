using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour, IDamageable
{
    public int health;
    public int maxHealth;

    [SerializeField] private int dropAmount;
    private EnemyStateMachine eStateMachine;
    public GameObject particlePrefab;
    public GameObject particlePrefabInstance;

    private void Start()
    {
        eStateMachine = GetComponent<EnemyStateMachine>();
        health = maxHealth;
    }
    public void OnTakeDamage(int damage)
    {
        health -= damage;
        particlePrefabInstance = Instantiate(particlePrefab, transform.position, transform.rotation);

        if (health <= 0)
        {
            OnDeath();
        }
    }


    public void OnDeath()
    {
        eStateMachine.IsAlive = false;
        Destroy(particlePrefabInstance, .3f);
        gameObject.SetActive(false);
        GetComponent<DropOnDeath>().DropLoot(dropAmount);
        if (TutorialManager.Instance != null)
        {
            if (TutorialManager.Instance.actualMission == null) return;
            if (TutorialManager.Instance.actualMission.missionType != MissionData.MissionType.DefeatEnemy) return;
            TutorialManager.Instance.DefeatMissionValidate();

        }

    }
}
