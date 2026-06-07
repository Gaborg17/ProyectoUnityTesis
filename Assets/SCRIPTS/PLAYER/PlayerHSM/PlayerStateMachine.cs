using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{
    private InputManager inputManager;
    private Rigidbody rb;
    private GroundChecker checker;

    [SerializeField] private Animator p_Animator;
    
    int _isWalkingHash;
    int _isAttackingHash;
    int _isJumpHash;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _jumpRequested;

    public bool JumpRequested { get { return _jumpRequested; } set { _jumpRequested = value; } }
    public float JumpForce { get { return _jumpForce; }}
    public float WalkSpeed {  get { return _walkSpeed; }}

    public Rigidbody Rb { get { return rb; }}
    public Animator PAnimator { get { return p_Animator; }}
    public GroundChecker GroundChecker { get { return checker; }}
    public InputManager InputManager { get { return inputManager; } }
    public Transform Transform { get { return transform; } }

    public int IsWalkingHash {  get { return _isWalkingHash; }}
    public int IsAttackingHash {  get { return _isAttackingHash; }}
    public int IsJumpHash { get { return _isJumpHash; }}

    PlayerBaseState _currentState;
    PlayerStateFactory _stateFactory;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}

    public bool isWalking;

    public GameObject temporalDamageCollider;
    private void Awake()
    {
        inputManager = InputManager.Instance;
        _stateFactory = new PlayerStateFactory(this);
        _currentState = _stateFactory.Grounded();
        _currentState.EnterState();
        rb = GetComponent<Rigidbody>();
        checker = GetComponent<GroundChecker>();

        _isWalkingHash = Animator.StringToHash("IsWalking");
        _isAttackingHash = Animator.StringToHash("Hit");
        _isJumpHash = Animator.StringToHash("Jump");
    }

    private void OnEnable()
    {
        
        if (inputManager != null)
        {
            inputManager.OnJumpPerformed += RequestJump;
            //inputManager.OnInteractPerformed += HandleInteraction;
            inputManager.OnInventoryPerformed += HandleInteraction;
        }

    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnJumpPerformed -= RequestJump;
            //inputManager.OnInteractPerformed -= HandleInteraction;
            inputManager.OnInventoryPerformed -= HandleInteraction;

        }
    }
    private void Start()
    {
        if (GameManager.Instance.islaSeleccionada != null)
        {
            transform.position = GameManager.Instance.islaSeleccionada.posicionDeSpawn;

        }
    }

    private void RequestJump()
    {
        if(checker.IsGrounded())
        _jumpRequested = true;
    }

    private void HandleInteraction()
    {
        SceneManager.LoadScene("MapaIslas");
    }

    private void FixedUpdate()
    {
        _currentState.UpdateStates();
        Movement();
        
        //HandleRotation();
    }


    private void Movement()
    {
        rb.linearVelocity = transform.localRotation * new Vector3(inputManager.MoveDirection().x * Speed(), rb.linearVelocity.y, inputManager.MoveDirection().y * Speed());
    }

    private float Speed()
    {
        return _walkSpeed * Time.deltaTime * 100f;
    }


    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = inputManager.MoveDirection().x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = inputManager.MoveDirection().y;

        Quaternion currentRotation = transform.rotation;


        if(inputManager.MoveDirection().magnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 10 * Time.deltaTime);
        }

    }
}
