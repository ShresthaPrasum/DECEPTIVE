using System;
using System.Collections;
using UnityEngine;

public class Warpe : MonoBehaviour
{
    public Transform destination;
    [SerializeField] private float teleportDelay = 0.25f;

    [SerializeField] private AudioClip audioClip;

    private AudioSource audioSource;

    public new ParticleSystem particleSystem;

    private bool isTeleporting;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }
        else{
            var sr = collision.GetComponent<SpriteRenderer>();
            sr.enabled = false;

            var rb = collision.attachedRigidbody;
            rb.simulated = false;

            audioSource.PlayOneShot(audioClip);

        }

        if (teleportDelay <= 0f)
        {
            collision.transform.position = destination.position;
            return;
        }

        if (isTeleporting) return;

        StartCoroutine(TeleportAfterDelay(collision.transform,collision.gameObject));
    }

    private IEnumerator TeleportAfterDelay(Transform target, GameObject player)
    {
        isTeleporting = true;
        yield return new WaitForSeconds(teleportDelay);

        if (target != null && destination != null)
        {
            target.position = destination.position;
            
            var sr = player.GetComponent<SpriteRenderer>();
            

            var rb = player.GetComponent<Rigidbody2D>();

            DoAfterDelay(0.2f, () =>
            {
                sr.enabled = true;
                rb.simulated = true;
            });
            
            particleSystem.Play();
        }

        isTeleporting = false;
    }

    public void DoAfterDelay(float delaySeconds, Action action)
    {
        StartCoroutine(DoAfterDelayRoutine(delaySeconds, action));
    }

    private IEnumerator DoAfterDelayRoutine(float delaySeconds, Action action)
    {
        if (delaySeconds > 0f)
        {
            yield return new WaitForSeconds(delaySeconds);
        }

        action?.Invoke();
    }



}