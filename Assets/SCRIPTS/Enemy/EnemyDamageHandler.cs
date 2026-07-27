using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    [SerializeField]private int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().OnTakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
