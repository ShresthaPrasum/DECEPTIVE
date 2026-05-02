using UnityEngine;

public class SpriteRendererOff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {      
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}