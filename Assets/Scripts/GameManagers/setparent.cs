using System;
using Unity.VisualScripting;
using UnityEngine;

public class Parent: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {

        if (collision.CompareTag("Player")) 
        {
            collision.transform.SetParent(transform);

            Collider2D playerCollider = collision.GetComponent<Collider2D>();

            SpriteRenderer playerSprite = collision.GetComponent<SpriteRenderer>();

            playerCollider.enabled = false;

            playerSprite.transform.position = transform.position;
        }
    }
}