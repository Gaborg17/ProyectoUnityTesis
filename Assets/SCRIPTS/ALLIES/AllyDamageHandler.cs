using UnityEngine;

public class AllyDamageHandler : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamageable>().OnTakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
