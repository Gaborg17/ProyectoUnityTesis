using UnityEngine;
using UnityEngine.SceneManagement;

public class NpcHealthHandler : MonoBehaviour, IDamageable
{
    public int health;
    public int maxHealth;

    [SerializeField] private int dropAmount;

    private NpcStateMachine stateMachine;

    private void Start()
    {
        stateMachine = GetComponent<NpcStateMachine>();
        health = maxHealth;
    }
    public void OnTakeDamage(int damage)
    {
        health -= damage;
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
    }
}
