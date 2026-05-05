using Unity.VisualScripting;
using UnityEngine;


public class changeDirection : MonoBehaviour
{
    public flappy flappy;

    public bool moveRight = true;
    
    [SerializeField] private AudioClip audioClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            flappy.moveRight = moveRight;

            audioSource.PlayOneShot(audioClip);
        }
    } 
}