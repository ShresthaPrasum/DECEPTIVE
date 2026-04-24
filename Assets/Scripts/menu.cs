using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    private bool showingMenu = false;
    [SerializeField] private GameObject escMenu;

    [SerializeField] private AudioClip audioClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
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