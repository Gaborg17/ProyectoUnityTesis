using System.Collections;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private EnemyStateMachine eStateMAchine;
    private Coroutine coroutine;
    [SerializeField] private float timeToStop;

    private SphereCollider detectionCollider;

    private void Start()
    {
        eStateMAchine = GetComponentInParent<EnemyStateMachine>();
        detectionCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            detectionCollider.radius = 15;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            
            eStateMAchine.CombatManager.MatchUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(coroutine == null) coroutine = StartCoroutine(StopChasing());
            
        }
    }


    private IEnumerator StopChasing()
    {
        yield return new WaitForSeconds(timeToStop);
        detectionCollider.radius = 2.5f;
        eStateMAchine.InCombat = false;
        coroutine = null;
    }

    private void OnDisable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }
}
