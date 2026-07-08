using UnityEngine;

public class PlayerHealthHandler : MonoBehaviour, IDamageable
{
    [SerializeField] protected int _playerHealth;
    [SerializeField] protected int _playerMaxHealth;

    public int PlayerHealth {  get { return _playerHealth; } set { _playerHealth = value; } }
    public int PlayerMaxHealth { get { return _playerMaxHealth; } }


    void Start()
    {
        _playerHealth = _playerMaxHealth;
        GameManager.Instance.UpdateHealthUI(PlayerHealth);
    }

    void Update()
    {
        
    }

    public void OnTakeDamage(int damage)
    {
        _playerHealth -= damage;
        GameManager.Instance.UpdateHealthUI(PlayerHealth);
        if ( _playerHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        Debug.Log("Player Died");
    }
}
