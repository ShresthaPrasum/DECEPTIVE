using UnityEngine;

public class NullingParent: MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.collider.CompareTag("Player")) 
        {
            collision.collider.transform.SetParent(null);
        }
    }
}