using UnityEngine;

public class EnemyHealthHandler : MonoBehaviour, IDamageable
{
    public int health;
    public int maxHealth;

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
        gameObject.SetActive(false);
    }
}
