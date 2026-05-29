using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GroundChecker), typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private InputManager inputManager;
    private Rigidbody rb;
    private GroundChecker checker;

    [SerializeField] private Animator p_Animator;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool _jumpRequested;

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        checker = GetComponent<GroundChecker>();


    }
    private void OnEnable()
    {
        inputManager = InputManager.Instance;
        if (inputManager != null)
        {
            inputManager.OnJumpPerformed += RequestJump;
            inputManager.OnInteractPerformed += HandleInteraction;
        }

    }

    private void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.OnJumpPerformed -= RequestJump;
            inputManager.OnInteractPerformed -= HandleInteraction;
        }
    }
    private void Start()
    {
        if (GameManager.Instance.islaSeleccionada != null)
        {
            transform.position = GameManager.Instance.islaSeleccionada.posicionDeSpawn;

        }
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();

    }


    private void Movement()
    {
        rb.linearVelocity = transform.localRotation * new Vector3(inputManager.MoveDirection().x * Speed(), rb.linearVelocity.y, inputManager.MoveDirection().y * Speed());
    }

    private float Speed()
    {
        return walkSpeed * Time.deltaTime * 100f;
    }

    private void Jump()
    {
        if (_jumpRequested && checker.IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpRequested = false;
        }
    }

    private void RequestJump()
    {
        _jumpRequested = true;
    }

    private void HandleInteraction()
    {
        SceneManager.LoadScene("MapaIslas");
    }
}
