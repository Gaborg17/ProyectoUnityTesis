using UnityEngine;

public class AllyHealthHandler : MonoBehaviour, IDamageable
{
    public int health;
    public int maxHealth;

    private AllyStateMachine allyStateMAchine;

    private void Start()
    {
        allyStateMAchine = GetComponent<AllyStateMachine>();
        health = maxHealth;
    }
    public void OnTakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            OnDeath();
        }
    }


    public void OnDeath()
    {
        allyStateMAchine.IsAlive = false;
        gameObject.SetActive(false);
    }
}
