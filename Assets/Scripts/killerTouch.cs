using UnityEngine;
using UnityEngine.SceneManagement;
using FirstGearGames.SmoothCameraShaker;

public class KillPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    [SerializeField] private GameObject particle;

    [SerializeField] private SpriteRenderer playerSprite;

    [SerializeField] private float loadDelay = 0.2f;

    public ShakeData CameraShaker;
     
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioClip);
            playerSprite.enabled = false;
            particle.SetActive(true);
            CameraShakerHandler.Shake(CameraShaker);
            Invoke(nameof(RestartLevel), loadDelay);
        }
    }

    private void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}