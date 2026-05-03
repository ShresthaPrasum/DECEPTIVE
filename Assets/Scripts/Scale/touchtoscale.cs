using Unity.VisualScripting;
using UnityEngine;

public class TouchToScale : MonoBehaviour
{
	public enum ScaleDirection
	{
		Increase,
		Decrease
	}

	public enum ScaleMode
	{
		HoldToScale,
		TouchToScale
	}

	[Header("Target")]
	[SerializeField] private Transform target;
	[SerializeField] private string playerTag = "Player";

	[Header("Mode")]
	[SerializeField] private ScaleMode mode = ScaleMode.HoldToScale;
	[SerializeField] private ScaleDirection direction = ScaleDirection.Increase;

	[Header("Scale")]

	[SerializeField] private float scaleRate = 1f;

	[SerializeField] private Vector3 scaleOffset = new Vector3(1f, 1f, 1f);
	[SerializeField] private bool disableAfterTouch = true;

	private AudioSource audioSource;

	[SerializeField] private AudioClip audioClip;

	private bool playerInside;
	private bool isScaling;
	private bool hasCompleted;
	private Vector3 remainingOffset;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    private void Update()
	{
		if (target == null) return;

		if (mode == ScaleMode.HoldToScale)
		{
			if (!playerInside) return;

			float sign = direction == ScaleDirection.Decrease ? -1f : 1f;
			Vector3 delta = Vector3.one * (scaleRate * Time.deltaTime * sign);
			target.localScale += delta;
			return;
		}

		if (!isScaling) return;

		Vector3 step = new Vector3(
			Mathf.MoveTowards(0f, remainingOffset.x, Mathf.Abs(scaleRate) * Time.deltaTime),
			Mathf.MoveTowards(0f, remainingOffset.y, Mathf.Abs(scaleRate) * Time.deltaTime),
			Mathf.MoveTowards(0f, remainingOffset.z, Mathf.Abs(scaleRate) * Time.deltaTime)
		);

		target.localScale += step;
		remainingOffset -= step;

		if (remainingOffset.sqrMagnitude <= 0.0001f)
		{
			remainingOffset = Vector3.zero;
			isScaling = false;
			hasCompleted = true;

			if (disableAfterTouch)
			{
				enabled = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		HandleEnter(other.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		HandleEnter(other.gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		HandleExit(other.gameObject);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		HandleExit(other.gameObject);
	}

	private void HandleEnter(GameObject other)
	{
		if (!other.CompareTag(playerTag)) return;

		if (target == null)
		{
			target = other.transform;
		}

		playerInside = true;

		if (mode != ScaleMode.TouchToScale) return;
		if (hasCompleted || isScaling) return;

		float sign = direction == ScaleDirection.Decrease ? -1f : 1f;
		remainingOffset = scaleOffset * sign;
		isScaling = true;

		audioSource.PlayOneShot(audioClip);
	}

	private void HandleExit(GameObject other)
	{
		if (!other.CompareTag(playerTag)) return;
		playerInside = false;
	}
}
