using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class jumpingSpikes : MonoBehaviour
{
    
    private bool isGrounded;

    [SerializeField] private float jumpForce= 7f; 

    [Header("Jump")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.06f;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundRadius,groundLayer);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    [Obsolete]
    private void Update()
    {
        CheckGround();

        if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}