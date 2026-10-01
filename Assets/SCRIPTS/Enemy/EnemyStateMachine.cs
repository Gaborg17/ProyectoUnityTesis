using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private Animator enemyAnim;

    private NavMeshAgent agent;

    protected Transform player;

    [SerializeField] private Transform[] destinos;
    private int destino;

    [SerializeField] private bool _isWalking;

    [SerializeField]protected bool _isInCombat;

    [SerializeField] protected BoxCollider _damageZone;


    public Animator EnemyAnimator { get { return enemyAnim; } }
    public NavMeshAgent Agent { get { return agent; } }

    public Transform[] Destinos { get { return destinos; } }
    public int Destino { get { return destino; } set { destino = value; } }
    public Transform Player {  get { return player; } }

    public bool isWalking { get { return _isWalking; } set { _isWalking = value; } }

    public bool InCombat { get { return _isInCombat; } set { _isInCombat = value; } }

    public BoxCollider  DamageZone {  get { return _damageZone; } }

    EnemyBaseState _currentState;
    EnemyStateFactory _stateFactory;

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public Coroutine waiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _stateFactory = new EnemyStateFactory(this);
        _currentState = _stateFactory.Grounded();
        _currentState.EnterStates();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        _currentState.UpdateStates();
    }

    private IEnumerator WaitToChange()
    {
        Destino = RandomNum();
        yield return new WaitForSeconds(4);
        Agent.isStopped = false;
    }

    public void ChangeDestiny()
    {
        waiting = StartCoroutine(WaitToChange());

    }

    public void CancelChange()
    {
        if(waiting != null)
        {
            StopCoroutine(waiting);
        }
        waiting = null;
    }

    private int RandomNum()
    {
        int randomNum = Random.Range(0, Destinos.Length - 1);
        return randomNum;
    }
}
