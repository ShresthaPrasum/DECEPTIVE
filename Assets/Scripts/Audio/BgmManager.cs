using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BgmManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuClip;
    [SerializeField] private AudioClip levelClip;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;
    [SerializeField, Min(0f)] private float fadeDuration = 1f;

    [Header("Scene Rules")]
    [SerializeField] private string levelScenePrefix = "Level";
    [SerializeField] private string[] levelSceneNames;

    [Header("Startup")]
    [SerializeField] private bool playOnAwake = true;

    private static BgmManager instance;
    private float baseVolume;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        baseVolume = Mathf.Clamp01(volume);
        audioSource.volume = baseVolume;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void Start()
    {
        if (playOnAwake)
        {
            HandleSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip nextClip = GetClipForScene(scene.name);
        if (nextClip == null)
        {
            return;
        }

        if (audioSource.clip == nextClip && audioSource.isPlaying)
        {
            return;
        }

        PlayWithFade(nextClip);
    }

    private void PlayWithFade(AudioClip nextClip)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        if (audioSource.clip == null || !audioSource.isPlaying || fadeDuration <= 0f)
        {
            audioSource.clip = nextClip;
            audioSource.volume = baseVolume;
            audioSource.Play();
            return;
        }

        fadeRoutine = StartCoroutine(FadeTransition(nextClip));
    }

    private IEnumerator FadeTransition(AudioClip nextClip)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = nextClip;
        audioSource.Play();

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, baseVolume, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = baseVolume;
        fadeRoutine = null;
    }

    private AudioClip GetClipForScene(string sceneName)
    {
        if (IsLevelScene(sceneName))
        {
            return levelClip != null ? levelClip : menuClip;
        }

        return menuClip;
    }

    private bool IsLevelScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(levelScenePrefix) &&
            sceneName.StartsWith(levelScenePrefix, StringComparison.Ordinal))
        {
            return true;
        }

        if (levelSceneNames == null)
        {
            return false;
        }

        foreach (string name in levelSceneNames)
        {
            if (!string.IsNullOrEmpty(name) &&
                sceneName.Equals(name, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}
