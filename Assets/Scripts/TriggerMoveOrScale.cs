using UnityEngine;

public class TriggerToMove : MonoBehaviour
{
    public enum TriggerAction
    {
        Move,
        Scale
    }

    [Header("Action")]
    [SerializeField] private TriggerAction action = TriggerAction.Move;
    [SerializeField] private Transform target;
    [SerializeField] private string playerTag = "Player";

    [Header("Move")]
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool useLocalPosition = false;

    [Header("Scale")]
    [SerializeField] private Vector3 scaleOffset = new Vector3(1f, 1f, 1f);
    [SerializeField] private float scaleSpeed = 3f;

    [Header("Reset")]
    [SerializeField] private bool resetOnExit = false;

    private Vector3 startPosition;
    private Vector3 startScale;
    private Vector3 targetPosition;
    private Vector3 targetScale;

    private bool isRunning;
    private bool hasState;

    private void Update()
    {
        if (!isRunning || target == null) return;

        if (action == TriggerAction.Move)
        {
            Vector3 current = useLocalPosition ? target.localPosition : target.position;
            Vector3 next = Vector3.MoveTowards(current, targetPosition, moveSpeed * Time.deltaTime);
            SetPosition(next);

            if ((next - targetPosition).sqrMagnitude <= 0.0001f)
            {
                SetPosition(targetPosition);
                isRunning = false;
            }
        }
        else
        {
            Vector3 next = Vector3.MoveTowards(target.localScale, targetScale, scaleSpeed * Time.deltaTime);
            target.localScale = next;

            if ((next - targetScale).sqrMagnitude <= 0.0001f)
            {
                target.localScale = targetScale;
                isRunning = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryStart(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryStart(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        TryReset(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TryReset(other.gameObject);
    }

    private void TryStart(GameObject other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (isRunning) return;

        if (target == null)
        {
            target = transform;
        }

        // Capture state at trigger time so any pre-move is respected.
        startPosition = useLocalPosition ? target.localPosition : target.position;
        startScale = target.localScale;

        targetPosition = startPosition + moveOffset;
        targetScale = startScale + scaleOffset;

        isRunning = true;
        hasState = true;
    }

    private void TryReset(GameObject other)
    {
        if (!resetOnExit) return;
        if (!other.CompareTag(playerTag)) return;
        if (!hasState || target == null) return;

        isRunning = false;

        if (action == TriggerAction.Move)
        {
            SetPosition(startPosition);
        }
        else
        {
            target.localScale = startScale;
        }
    }

    private void SetPosition(Vector3 value)
    {
        if (useLocalPosition)
        {
            target.localPosition = value;
        }
        else
        {
            target.position = value;
        }
    }
}
