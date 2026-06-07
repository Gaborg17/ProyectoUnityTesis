using System.Collections;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private EnemyStateMachine eStateMAchine;
    private Coroutine coroutine;
    [SerializeField] private float timeToStop;

    private void Start()
    {
        eStateMAchine = GetComponentInParent<EnemyStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            eStateMAchine.InCombat = true;
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
        eStateMAchine.InCombat = false;
        coroutine = null;
    }

    private void OnDisable()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }
}
