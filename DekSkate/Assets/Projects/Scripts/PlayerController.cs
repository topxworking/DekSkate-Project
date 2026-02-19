using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Animator animator;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float gravityScale = 6f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 _moveInput;
    private bool _isGrounded;
    private bool _isDead = false;
    private PlayerControls _inputs;

    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int DieHash = Animator.StringToHash("Die");
    private static readonly int VerticalVelHash = Animator.StringToHash("VerticalVelocity");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

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
        if (_isDead) return;

        _isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        animator.SetBool(IsGroundedHash, _isGrounded);
        animator.SetFloat(VerticalVelHash, rb.linearVelocity.y);

        UpdateAnimations();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        if (_isDead) return;

        rb.linearVelocity = new Vector3(_moveInput.x * moveSpeed, rb.linearVelocity.y, 0);
        rb.AddForce(Physics.gravity * (gravityScale - 1) , ForceMode.Acceleration);
    }

    private void HandleMovement()
    {
        if (_moveInput.x > 0.01f)
        {
            transform.eulerAngles = new Vector3(0, 90f, 0);
        }
        else if (_moveInput.x < -0.01f)
        {
            transform.eulerAngles = new Vector3(0, -90f, 0);
        }
    }

    private void UpdateAnimations()
    {
        bool isWalking = Mathf.Abs(_moveInput.x) > 0.01f;
        animator.SetBool(IsWalkingHash, isWalking);
    }

    private void Jump()
    {
        if (_isDead) return;

        if (_isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger(JumpHash);
        }
    }

    private void JumpCut()
    {
        if (_isDead) return;

        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    private void Die()
    {
        Time.timeScale = 0f;
        _isDead = true;

        animator.SetTrigger(DieHash);
        rb.linearVelocity = Vector3.zero;

        _inputs.Disable();
    }
}
