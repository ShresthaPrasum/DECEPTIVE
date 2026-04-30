using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using FirstGearGames.SmoothCameraShaker;

public class ChangeControl : MonoBehaviour
{
    public GameObject playerObject;

    private bool enabled = false;

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
        if (collision.gameObject.CompareTag("Player") && enabled == false)
        {          
            playerScript.right = right;
            playerScript.left = left;
            audioSource.PlayOneShot(audioClip);
            CameraShakerHandler.Shake(CameraShaker);
            enabled = true;
        }

    }
}

