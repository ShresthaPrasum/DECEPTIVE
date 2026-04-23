using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    private bool isGrounded;

    private bool isRunning;

    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed= 4f; 
    [SerializeField] private float jumpForce= 7f; 

    [Header("Jump")]

    private Rigidbody2D rb;

    private SpriteRenderer _spriteRender;

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

        if(Input.GetKey(KeyCode.A))
        {
            move = -1f;
            _spriteRender.flipY = true;
            isRunning = true;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            move = 1f;
            _spriteRender.flipY = false;
            isRunning = true;
        }

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        animator.SetBool("isRunning", isRunning);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.isTrigger)
        {
            isGrounded = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(!collision.collider.isTrigger)
        {
            isGrounded = false;
        }
    }
}
