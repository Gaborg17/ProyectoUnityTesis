using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private InputManager inputManager;
    private Rigidbody rb;
    private GroundChecker checker;

    [SerializeField] private Animator p_Animator;
    [SerializeField] private Transform Camera;

    int _isWalkingHash;
    int _isAttackingHash;
    int _isJumpHash;
    int _isRunningHash;

    [Header("Movement")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    private float _actualSpeed;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _jumpRequested;

    [Header("Stats")]
    [SerializeField] private int _damage;
    public bool JumpRequested { get { return _jumpRequested; } set { _jumpRequested = value; } }
    public float JumpForce { get { return _jumpForce; } }
    public float WalkSpeed { get { return _walkSpeed; } }
    public float RunSpeed { get { return _runSpeed; } }
    public float ActualSpeed { get { return _actualSpeed; } set { _actualSpeed = value; } }

    public int Damage { get { return _damage; } }

    public Rigidbody Rb { get { return rb; } }
    public Animator PAnimator { get { return p_Animator; } }
    public GroundChecker GroundChecker { get { return checker; } }
    public InputManager InputManager { get { return inputManager; } }
    public Transform Transform { get { return transform; } }

    public int IsWalkingHash { get { return _isWalkingHash; } }
    public int IsAttackingHash { get { return _isAttackingHash; } }
    public int IsJumpHash { get { return _isJumpHash; } }
    public int IsRunningHash { get { return _isRunningHash; } }

    PlayerBaseState _currentState;
    PlayerStateFactory _stateFactory;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

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
        _isRunningHash = Animator.StringToHash("Run");
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
        inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            inputManager.OnJumpPerformed += RequestJump;
            //inputManager.OnInteractPerformed += HandleInteraction;
            inputManager.OnInventoryPerformed += HandleInteraction;
        }
        if (GameManager.Instance.islaSeleccionada != null)
        {
            transform.position = GameManager.Instance.islaSeleccionada.posicionDeSpawn;

        }
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.OnJumpPerformed -= RequestJump;
            //inputManager.OnInteractPerformed -= HandleInteraction;
            inputManager.OnInventoryPerformed -= HandleInteraction;

        }
    }

    private void RequestJump()
    {
        if (checker.IsGrounded())
            _jumpRequested = true;
    }

    private void HandleInteraction()
    {
        //SceneManager.LoadScene("MapaIslas");
    }

    private void FixedUpdate()
    {
        _currentState.UpdateStates();

        HandleRotation();
        Movement();
    }

    public bool CanEnterState(PlayerBaseState target)
    {
        if (_currentState is PlayerJumpState && target is PlayerInCombatState)
            return false;

        return true;
    }
    private void Movement()
    {
        Vector3 direction = CameraDirection();

        if (direction.magnitude > 0.05f)
        {
            Vector3 velocity = CameraDirection() * Speed();

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 0.6f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                float wallAngle = Vector3.Angle(Vector3.up, hit.normal);

                if (wallAngle > 45f && wallAngle < 92f)
                {
                    velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
                }
            }

            velocity.y = rb.linearVelocity.y;
            rb.linearVelocity = velocity;

            rb.useGravity = true;
        }
        else
        {
            if (checker.IsGrounded())
            {
                rb.linearVelocity = Vector3.zero;
                rb.useGravity = false;
            }
            else
            {
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                rb.useGravity = true;
            }
        }


    }

    private float Speed()
    {
        
        return _actualSpeed * Time.deltaTime * 100f;
    }

    private Quaternion targetRotation;
    private void HandleRotation()
    {

        if (inputManager.MoveDirection().sqrMagnitude > 0.01f)
        {

            targetRotation = Quaternion.LookRotation(CameraDirection());

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                540f * Time.deltaTime
            );

        }

    }

    private Vector3 CameraDirection()
    {
        Vector2 input = inputManager.MoveDirection();
        Vector3 frwd = Camera.forward;
        Vector3 right = Camera.right;

        frwd.y = 0;
        right.y = 0;

        frwd.Normalize();
        right.Normalize();

        Vector3 direction = frwd * input.y + right * input.x;
        if (direction.sqrMagnitude < 0.01f) return Vector3.zero;
        return direction.normalized;
    }

}
