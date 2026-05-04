using UnityEngine;

public class SpinPauseTrigger : MonoBehaviour
{
    [SerializeField] private SpinForever spinTarget;
    [SerializeField] private float pauseSeconds = 1f;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        TryPause(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryPause(other.gameObject);
    }

    private void TryPause(GameObject other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (spinTarget == null) return;

        spinTarget.PauseForSeconds(pauseSeconds);
    }
}
