using Unity.VisualScripting;
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

    private void Start()
    {
        inputManager = InputManager.Instance;
        rb = GetComponent<Rigidbody>();
        checker = GetComponent<GroundChecker>();

    }

    private void FixedUpdate()
    {
        Movement();
        Jump();

        if (inputManager.Interact())
        {
            SceneManager.LoadScene("MapaIslas");
        }
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
        if (inputManager.isJumpPressed() && checker.IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        }
    }
}
