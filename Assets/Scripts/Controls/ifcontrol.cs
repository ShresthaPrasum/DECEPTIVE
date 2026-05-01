using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;

public class IfChangeControl : MonoBehaviour
{
    public GameObject playerObject;

    private bool hasChanged = false;

    [SerializeField] public Key right = Key.D;
    [SerializeField] public Key left = Key.A;

    private Player playerScript;

    public ShakeData CameraShaker;

    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    void Awake()
    {
        playerScript = playerObject.GetComponent<Player>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && hasChanged == false)
        {          
            if (playerScript.right == Key.D)
            {
                playerScript.right = left;
            }
            else
            {
                playerScript.right = right;
            }
            if (playerScript.left == Key.A)
            {
                playerScript.left = right;
            }
            else
            {
                playerScript.left = left;
            }
            audioSource.PlayOneShot(audioClip);
            CameraShakerHandler.Shake(CameraShaker);
            hasChanged = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && hasChanged == false)
        {          
            if (playerScript.right == Key.D)
            {
                playerScript.right = left;
            }
            else
            {
                playerScript.right = right;
            }
            if (playerScript.left == Key.A)
            {
                playerScript.left = right;
            }
            else
            {
                playerScript.left = left;
            }
            audioSource.PlayOneShot(audioClip);
            CameraShakerHandler.Shake(CameraShaker);
            hasChanged = true;
        }

    }
}

