using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private Animator enemyAnim;

    private NavMeshAgent agent;

    [SerializeField] private Transform[] destinos;

    [SerializeField]private bool _isWalking;


    public Animator EnemyAnimator { get { return enemyAnim; } }
    public NavMeshAgent Agent { get { return agent; } }

    public Transform[] Destinos { get { return destinos; } }

    public bool isWalking { get { return _isWalking; } set { _isWalking = value; } }

    EnemyBaseState _currentState;
    EnemyStateFactory _stateFactory;

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _stateFactory = new EnemyStateFactory(this);
        _currentState = _stateFactory.Grounded();
        _currentState.EnterState();
    }
    private void Update()
    {
        _currentState.UpdateStates();
    }
}
