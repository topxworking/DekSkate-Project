using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public PlayerData playerData;

    [Header("Intro UI")]
    public GameObject introPanel;
    public CanvasGroup introCanvasGroup;
    public float fadeDuration = 1f;
    public TextMeshProUGUI introLevelText;
    public TextMeshProUGUI introLifeText;
    public TextMeshProUGUI hudLifeText;

    [Header("Component")]
    public Animator animator;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public ParticleSystem moveDust;

    [Header("Jump Settings")]
    public float jumpForce = 20f;
    public float jumpCutMultiplier = 0.5f;
    public float gravityScale = 6f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Life System")]
    private string lifePrefix = "x ";
    private int currentLife;
    public int maxLife = 3;
    public GameObject gameOverUI;
    private Vector3 startPosition;

    [Header("Audio Settings")]
    public AudioSource walkAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip dieSound;
    public AudioClip walkSound;
    public AudioClip jumpSound;

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
        _inputs = new PlayerControls();

        rb = GetComponent<Rigidbody>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (playerData != null)
        {
            currentLife = playerData.currentLife;
        }

        if (introCanvasGroup != null)
        {
            StartCoroutine(LevelIntro());
        }

        startPosition = transform.position;
        UpdateUI();
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

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
        bool isWalking = Mathf.Abs(_moveInput.x) > 0.01f && _isGrounded;
        animator.SetBool(IsWalkingHash, isWalking);

        if (isWalking)
        {
            if (!moveDust.isPlaying)
            {
                moveDust.Play();
            }

            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.loop = true;
                walkAudioSource.Play();
            }
        }
        else
        {
            if (moveDust.isPlaying)
            {
                moveDust.Stop();
            }

            if (walkAudioSource.isPlaying)
            {
                walkAudioSource.Stop();
            }
        }
    }

    private void Jump()
    {
        if (_isDead) return;

        if (_isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger(JumpHash);

            if (jumpSound != null)
            {
                sfxAudioSource.PlayOneShot(jumpSound);
            }
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
        if (_isDead) return;
        _isDead = true;

        walkAudioSource.Stop();
        if (dieSound != null)
        {
            sfxAudioSource.PlayOneShot(dieSound);
        }

        playerData.currentLife--;
        UpdateUI();

        if (playerData.currentLife > 0)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            GameOver();
        }

        animator.SetTrigger(DieHash);
        rb.linearVelocity = Vector3.zero;
        _inputs.Disable();
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = startPosition;

        _isDead = false;
        _inputs.Enable();

        animator.Rebind();
        animator.Update(0f);
    }

    private void UpdateUI()
    {
        if (hudLifeText != null)
        {
            hudLifeText.text = lifePrefix + playerData.currentLife;
        }
    }

    private void GameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            playerData.ResetData();
        }

        Time.timeScale = 0f;
    }

    private IEnumerator LevelIntro()
    {
        introCanvasGroup.alpha = 1f;
        introLevelText.text = SceneManager.GetActiveScene().name;
        introLifeText.text = lifePrefix + playerData.currentLife;

        _inputs.Disable();

        yield return new WaitForSecondsRealtime(2f);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            introCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        introCanvasGroup.alpha = 0f;
        introCanvasGroup.gameObject.SetActive(false);
        _inputs.Enable();
    }
}
