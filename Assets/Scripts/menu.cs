using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public bool showingMenu = false;
    [SerializeField] private GameObject escMenu;

    [SerializeField] private AudioClip audioClip;

    private AudioSource audioSource;

    private void Update()
    {
        if(Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            audioSource.PlayOneShot(audioClip);

            if (!showingMenu)
            {
                showingMenu = true;
                escMenu.SetActive(true);
            }

            else
            {
                showingMenu = false;
                escMenu.SetActive(false);
            }
           
        }
    }

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (!showingMenu)
        {
            escMenu.SetActive(false);
        }
        else
        {
            escMenu.SetActive(true);
        }
    }

    
    public void EscMenu()
    {
        if (!showingMenu)
        {
            AudioPlay();
            escMenu.SetActive(true);
            showingMenu = true;
        }
        else
        {
            AudioPlay();
            escMenu.SetActive(false);
            showingMenu = false;
        }
    }

    public void Resume()
    {
        AudioPlay();
        escMenu.SetActive(false);
        showingMenu = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void AudioPlay()
    {
        audioSource.PlayOneShot(audioClip);
    }
}