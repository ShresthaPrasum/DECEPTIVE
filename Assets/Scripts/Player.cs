using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{

    private bool isGrounded;

    private bool isRunning;

    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed= 4f; 
    [SerializeField] private float jumpForce= 7f; 

    [SerializeField] private AudioSource audioSource;

    [Header("Jump")]

    private Rigidbody2D rb;

    private SpriteRenderer _spriteRender;

    private float stepTimer = 0f;
    public float stepDelay = 1f; 

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
    }
}

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        _spriteRender = GetComponent<SpriteRenderer>();
    }

    [Obsolete]
    private void Update()
    {
        
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
        }

        animator.SetBool("isRunning", isRunning);

        HandleMovementAudio(move);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.isTrigger) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.isTrigger) return;

        isGrounded = false;
    }

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
    }
}
}
