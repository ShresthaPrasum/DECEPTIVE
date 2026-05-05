using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Controls;

public class flappy : MonoBehaviour
{
    private bool isJumping;
    private bool hasSpawned = false;

    [SerializeField] private Animator animator;

    [SerializeField] public bool moveRight = true;

    [SerializeField] private GameObject spawnParticle;

    [SerializeField] private float moveSpeed= 4f; 


    [SerializeField] private AudioSource audioSource;

    private AudioSource sourceJump;
    private AudioSource sourceRestart;

    [SerializeField] private AudioClip audioJump;
    
    [SerializeField] private AudioClip audioRestart;

    [Header("Jump")]
    [SerializeField] private float jumpForce= 7f; 
    [SerializeField, Min(0.2f)] private float LoadDelay;

    private Rigidbody2D rb;

    private SpriteRenderer _spriteRender;

    [SerializeField] public float spriteEnableDelay = 0.8f; 

    [SerializeField] private GameObject escMenu;

    [SerializeField] private SpriteRenderer checkPoint;

    private bool hasStartedMove;

    public void RestartLevel()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Awake()
    {
        sourceJump = GetComponent<AudioSource>();
        
            if (sourceJump == null)
            {
                sourceJump = gameObject.AddComponent<AudioSource>();
            }

        sourceRestart = GetComponent<AudioSource>();
            if (sourceRestart == null)
            {
                sourceRestart = gameObject.AddComponent<AudioSource>();
            }
        
        rb = GetComponent<Rigidbody2D>();

        _spriteRender = GetComponent<SpriteRenderer>();

        _spriteRender.enabled = false;

        if(!hasSpawned)
        {
            spawnParticle.SetActive(true);
            
            Invoke(nameof(SpriteEnable), spriteEnableDelay);

            hasSpawned = true;
        }

    }

    [Obsolete]
    private void Update()
    {


        if(Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            sourceRestart.PlayOneShot(audioRestart);

            Invoke(nameof(RestartLevel), LoadDelay);
           
        }
        if (!hasStartedMove && Keyboard.current != null && IsStartKeyPressed())
        {
            hasStartedMove = true;
        }

        float move = 0f;
        if (hasStartedMove)
        {
            if (moveRight)
            {
                _spriteRender.flipX= true;
                move = 1f;
            }
            else
            {
                _spriteRender.flipX = false;
                move = -1f;
            }
        }

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            sourceJump.PlayOneShot(audioJump);

            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        animator.SetBool("isJumping", isJumping);
    }

    private bool IsStartKeyPressed()
    {
        if (Keyboard.current == null) return false;

        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (!key.wasPressedThisFrame) continue;
            if (key == Keyboard.current.escapeKey) continue;
            if (key == Keyboard.current.rKey) continue;
            return true;
        }

        return false;
    }

    private void SpriteEnable()
    {
        _spriteRender.enabled = true;
        _spriteRender.transform.position = checkPoint.transform.position;
    }

}
