using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader: MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Level2";

    [SerializeField] private string playerTag = "Player";

// COOK COOKIN G COOKING

    [SerializeField] private bool allowParentTagCheck = true;

    [SerializeField] private bool useTrigger = true;

    [SerializeField,Min(0f)] private float loadDelay = 0f;
    [SerializeField,Min(0f)] private float audioDelay = 0.2f;

    [SerializeField] private AudioClip outroSound;
    [SerializeField] private AudioClip doorSound;

    private AudioSource audioClip;
    private AudioSource doorClip;
    private Animator animator;

    private bool hasLoaded;

    private void Awake()
    {
        audioClip = GetComponent<AudioSource>();
            if (audioClip == null)
            {
                audioClip = gameObject.AddComponent<AudioSource>();
            }

            animator = gameObject.GetComponent<Animator>();

            doorClip = gameObject.AddComponent<AudioSource>();
    }

    private void LoadTargetScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!useTrigger) return;

        TryLoad(other.gameObject);

        Invoke(nameof(playSound), audioDelay);

        audioClip.PlayOneShot(outroSound);

        if(other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
        }

        animator.SetBool("isTouched", true);

    }


    private void playSound()
    {

        doorClip.PlayOneShot(doorSound);
    }
    private void TryLoad(GameObject candidate)
    {
        if(hasLoaded) return;

        if(!IsPlayerObject(candidate))
        return;

        if(string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogWarning("LevelLoader has no target scene set");

            return;
        }

        hasLoaded = true;

        if(loadDelay>0f)
        {
            Invoke(nameof(LoadTargetScene),loadDelay);
        }
        else
        {
            LoadTargetScene();
        }
    }

    private bool IsPlayerObject(GameObject candidate)
    {
        if(candidate == null) return false;

        if(!string.IsNullOrWhiteSpace(playerTag) && candidate.CompareTag(playerTag)) return true;

        if(allowParentTagCheck)
        {
            Transform current = candidate.transform.parent;

            while(current != null)
            {
                if(!string.IsNullOrWhiteSpace
                (playerTag) && current.CompareTag(playerTag)) return true;
            }
        }
        return false;
    }
}