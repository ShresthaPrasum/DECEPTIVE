using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{

    private bool isGrounded;

    private bool isRunning;
    private bool isJumping;

    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed= 4f; 
    [SerializeField] private float jumpForce= 7f; 

    [SerializeField] private AudioSource audioSource;

    [Header("Jump")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private SpriteRenderer _spriteRender;

    [SerializeField] private float stepTimer = 0f;
    [SerializeField] public float stepDelay = 5f; 

    void HandleMovementAudio(float moveInput)
    {
        if (moveInput != 0)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSource.Play();
                stepTimer = stepDelay;
            }
        }
        else
        {
            stepTimer = 0f; 
            audioSource.Stop();
        }
    }
    
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer
        );
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        _spriteRender = GetComponent<SpriteRenderer>();
    }

    [Obsolete]
    private void Update()
    {

        CheckGround();
        
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Home");
            return;
        }

        if(Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        float move = 0f;

        if (Keyboard.current.aKey.isPressed)
        {
            _spriteRender.flipX = false;
            move = -1f;
            isRunning = true;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            _spriteRender.flipX= true;
            move = 1f;
            isRunning = true;
        }

        else
        {
            isRunning = false;
            move = 0f;
        }

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);

        HandleMovementAudio(move);
    }

   
}
