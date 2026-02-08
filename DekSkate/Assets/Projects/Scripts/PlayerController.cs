using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float garvityScale = 2.5f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 _moveInput;
    private bool _isGrounded;
    private PlayerControls _inputs;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _inputs = new PlayerControls();

        _inputs.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _inputs.Player.Move.canceled += ctx => _moveInput = Vector2.zero;

        _inputs.Player.Jump.performed += ctx => Jump();
        _inputs.Player.Jump.canceled += ctx => JumpCut();
    }

    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (_moveInput.x > 0.01f)
        {
            transform.eulerAngles = new Vector3(0, 90f, 0);
        }
        else if (_moveInput.x < -0.01f)
        {
            transform.eulerAngles = new Vector3(0, -90f, 0);
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        Vector3 targetVelocity = new Vector3(_moveInput.x * moveSpeed, rb.linearVelocity.y, 0);
        rb.linearVelocity = targetVelocity;
    }

    private void ApplyGravity()
    {
        if (rb.useGravity)
        {
            rb.AddForce(Physics.gravity * (garvityScale - 1) * rb.mass);
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void JumpCut()
    {
        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier, 0);
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
