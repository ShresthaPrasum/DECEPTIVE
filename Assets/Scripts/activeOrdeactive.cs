using Unity.VisualScripting;
using UnityEngine;

public class activeOrdeactive : MonoBehaviour
{
    [Header("TargetObject")]
    public GameObject targetObject;

    [SerializeField] private bool activate;

    private AudioSource audio;

    [SerializeField] private AudioClip audioClip;

    [SerializeField] private float loadDelay = 0.2f;


    private void Awake()
    {
        audio = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke(nameof(Activeordeactive), loadDelay);
        }
        
    }

    private void Activeordeactive()
    {
        targetObject.SetActive(activate);
        audio.PlayOneShot(audioClip);
    }
}