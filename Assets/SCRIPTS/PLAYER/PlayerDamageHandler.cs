using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    [SerializeField]private PlayerStateMachine playerStateMachine;
    void Awake()
    {
      playerStateMachine = transform.root.GetComponent<PlayerStateMachine>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamageable>().OnTakeDamage(playerStateMachine.Damage);
        }
    }
}
