using UnityEngine;

public class aimovement : MonoBehaviour
{
	[Header("Animator")]
	[SerializeField] private Animator animator;
	[SerializeField] private SpriteRenderer spriteRenderer;

	[Header("Auto Move")]
	[SerializeField] private float moveDistance = 0.5f;
	[SerializeField] private float moveSpeed = 1f;

	[Header("Auto Jump")]
	[SerializeField] private float jumpHeight = 0.4f;
	[SerializeField] private float jumpDuration = 0.6f;
	[SerializeField] private float jumpInterval = 2.5f;

	[Header("Audio")]
	[SerializeField] private AudioSource footstepSource;
	[SerializeField] private AudioSource jumpSource;
	[SerializeField] private AudioClip audioJump;
	[SerializeField] private float stepDelay = 0.5f;

	[Header("Position Settings")]
	[SerializeField] private bool useLocalPosition = true;

	private Vector3 startPos;
	private float stepTimer;
	private float jumpTimer;
	private float jumpCooldown;
	private bool isJumping;
	private bool isRunning;

	private void Awake()
	{
		startPos = useLocalPosition ? transform.localPosition : transform.position;
		stepTimer = 0f;
		jumpCooldown = jumpInterval;
	}

	private void OnDisable()
	{
		if (useLocalPosition)
		{
			transform.localPosition = startPos;
		}
		else
		{
			transform.position = startPos;
		}
	}

	private void Update()
	{
		float t = Time.time;
		float movePhase = t * moveSpeed;
		float x = Mathf.Sin(movePhase) * moveDistance;
		float moveDir = Mathf.Cos(movePhase);

		UpdateJump(Time.deltaTime);
		float y = isJumping ? Mathf.Sin(Mathf.Clamp01(jumpTimer / jumpDuration) * Mathf.PI) * jumpHeight : 0f;

		Vector3 offset = new Vector3(x, y, 0f);
		if (useLocalPosition)
		{
			transform.localPosition = startPos + offset;
		}
		else
		{
			transform.position = startPos + offset;
		}

		isRunning = moveDistance > 0f && moveSpeed > 0f;
		UpdateAnimator();
		UpdateFootstepAudio(isRunning && !isJumping);
		UpdateFacing(moveDir);
	}

	private void UpdateJump(float deltaTime)
	{
		if (jumpCooldown > 0f)
		{
			jumpCooldown -= deltaTime;
		}

		if (!isJumping && jumpCooldown <= 0f && jumpDuration > 0f)
		{
			isJumping = true;
			jumpTimer = 0f;
			jumpCooldown = jumpInterval;
			if (jumpSource != null && audioJump != null)
			{
				jumpSource.PlayOneShot(audioJump);
			}
		}

		if (isJumping)
		{
			jumpTimer += deltaTime;
			if (jumpTimer >= jumpDuration)
			{
				isJumping = false;
			}
		}
	}

	private void UpdateAnimator()
	{
		if (animator == null)
		{
			return;
		}

		animator.SetBool("isRunning", isRunning);
		animator.SetBool("isJumping", isJumping);
	}

	private void UpdateFootstepAudio(bool canStep)
	{
		if (footstepSource == null)
		{
			return;
		}

		if (canStep)
		{
			stepTimer -= Time.deltaTime;
			if (stepTimer <= 0f)
			{
				footstepSource.Play();
				stepTimer = stepDelay;
			}
		}
		else
		{
			stepTimer = 0f;
			if (footstepSource.isPlaying)
			{
				footstepSource.Stop();
			}
		}
	}

	private void UpdateFacing(float moveDir)
	{
		if (spriteRenderer == null)
		{
			return;
		}

		if (moveDir < -0.01f)
		{
			spriteRenderer.flipX = false;
		}
		else if (moveDir > 0.01f)
		{
			spriteRenderer.flipX = true;
		}
	}
}
