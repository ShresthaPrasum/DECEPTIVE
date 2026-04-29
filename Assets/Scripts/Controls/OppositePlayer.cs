using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class OppositePlayer : MonoBehaviour
{

    private bool isGrounded;

    private bool isRunning;
    private bool hasSpawned = false;

    [SerializeField] private Animator animator;

    [SerializeField] private Key right = Key.D;
    [SerializeField] private Key left = Key.A;

    [SerializeField] private GameObject spawnParticle;

    [SerializeField] private float moveSpeed= 4f; 
    [SerializeField] private float jumpForce= 7f; 

    [SerializeField] private AudioSource audioSource;

    private AudioSource sourceJump;
    private AudioSource sourceRestart;

    
    [SerializeField] private AudioClip audioRestart;


    [SerializeField, Min(0.2f)] private float LoadDelay;

    private Rigidbody2D rb;

    private SpriteRenderer _spriteRender;

    [SerializeField] private float stepTimer = 0f;
    [SerializeField] public float stepDelay = 5f; 
    [SerializeField] public float spriteEnableDelay = 0.8f; 

    [SerializeField] private GameObject escMenu;

    [SerializeField] private SpriteRenderer checkPoint;

    
    
    void HandleMovementAudio(float moveInput)
    {
        if (moveInput != 0 && isGrounded == true)
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
    

    public void RestartLevel()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Awake()
    {

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

        float move = 0f;

        if (Keyboard.current[left].isPressed)
        {
            _spriteRender.flipX = false;
            move = -1f;
            isRunning = true;
        }
        else if (Keyboard.current[right].isPressed)
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


        animator.SetBool("isRunning", isRunning);


        HandleMovementAudio(move);
    }

    private void SpriteEnable()
    {
        _spriteRender.enabled = true;
        _spriteRender.transform.position = checkPoint.transform.position;
    }

    
   
}
