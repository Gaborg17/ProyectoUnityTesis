using UnityEngine;
using UnityEngine.AI;

public class AllyStateMachine : MonoBehaviour
{
    [SerializeField] private Animator allyAnim;

    private NavMeshAgent agent;

    protected Transform player;
    protected Transform assignedEnemy;

    [SerializeField] private bool _isWalking;

    [SerializeField] protected bool _isInCombat;

    [SerializeField] protected BoxCollider _damageZone;

    private CombatZoneManager _cManager;


    public Animator AllyAnimator { get { return allyAnim; } }
    public NavMeshAgent Agent { get { return agent; } }

    public Transform Player { get { return player; } }
    public Transform AssignedEnemy { get { return assignedEnemy; } set { assignedEnemy = value; } }

    public bool isWalking { get { return _isWalking; } set { _isWalking = value; } }

    public bool InCombat { get { return _isInCombat; } set { _isInCombat = value; } }

    public BoxCollider DamageZone { get { return _damageZone; } }

    public CombatZoneManager CombatManager { get { return _cManager; } set { _cManager = value; } }

    AllyBaseState _currentState;
    AllyStateFactory _stateFactory;

    public AllyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public float playerDistance;
    public float playerSideStep;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _stateFactory = new AllyStateFactory(this);
        _currentState = _stateFactory.Idle();
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


}
