using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour, IDamageable
{
    public int health;
    public int maxHealth;

    [SerializeField] private int dropAmount;

    private void Start()
    {
        health = maxHealth;
    }
    public void OnTakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            OnDeath();
        }
    }


    public void OnDeath()
    {
        if (TutorialManager.Instance != null)
        {
            if (TutorialManager.Instance.actualMission.missionType != MissionData.MissionType.DefeatEnemy || TutorialManager.Instance.actualMission == null) return;
            TutorialManager.Instance.DefeatMissionValidate();

        }
        gameObject.SetActive(false);
        GetComponent<DropOnDeath>().DropLoot(dropAmount);


    }
}
