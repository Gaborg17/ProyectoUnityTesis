using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcHealthHandler : MonoBehaviour, IDamageable
{
    public int health;
    public int maxHealth;

    [SerializeField] private int dropAmount;

    private NpcStateMachine stateMachine;

    public GameObject particlePrefab;
    public GameObject particlePrefabInstance;

    private void Start()
    {
        stateMachine = GetComponent<NpcStateMachine>();
        health = maxHealth;
    }
    public void OnTakeDamage(int damage)
    {
        health -= damage;
        particlePrefabInstance = Instantiate(particlePrefab, transform.position, transform.rotation);
        if (!stateMachine.Escaping)
        {
            stateMachine.Escape();
        }
        if (health <= 0)
        {
            OnDeath();
        }
    }


    public void OnDeath()
    {
        gameObject.SetActive(false);
        GetComponent<DropOnDeath>().DropLoot(dropAmount);
        Destroy(particlePrefabInstance, 1f);
        if(TutorialManager.Instance != null )
        {
            if (TutorialManager.Instance.actualMission == null) return;
            if (TutorialManager.Instance.actualMission.missionType != MissionData.MissionType.DefeatNpc) return;
            TutorialManager.Instance.DefeatMissionValidate();

        }
    }
}
