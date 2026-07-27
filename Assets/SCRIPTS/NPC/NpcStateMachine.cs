using UnityEngine;
using UnityEngine.AI;

public class NpcStateMachine : MonoBehaviour
{
    [SerializeField] private Animator npcAnim;

    private NavMeshAgent agent;

    protected Transform player;

    [SerializeField] private Transform[] destinos;

    [SerializeField] private bool _isWalking;

    [SerializeField] protected bool _isEscaping;




    public Animator NpcAnimator { get { return npcAnim; } }
    public NavMeshAgent Agent { get { return agent; } }

    public Transform[] Destinos { get { return destinos; } }
    public Transform Player { get { return player; } }

    public bool isWalking { get { return _isWalking; } set { _isWalking = value; } }

    public bool Escaping { get { return _isEscaping; } set { _isEscaping = value; } }



    NpcBaseState _currentState;
    NpcStateFactory _stateFactory;

    public NpcBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _stateFactory = new NpcStateFactory(this);
        _currentState = _stateFactory.Walk();
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

    public void Escape()
    {
        if (!_isEscaping)
        {
            _isEscaping = true;
            Debug.Log("aaaaaaaa");
        }
    }
}
