using System.Collections;
using UnityEngine;

public class TriggerMoveReturn : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private Transform moveTarget;
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool useLocalPosition = false;

    [Header("Trigger")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool resetOnPlayerExit = true;

    private Vector3 startPosition;
    private Coroutine moveCoroutine;
    private bool hasTriggered;

    private void Awake()
    {
        if (moveTarget == null)
        {
            moveTarget = transform;
        }

        startPosition = useLocalPosition ? moveTarget.localPosition : moveTarget.position;
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
        ResetTrigger(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ResetTrigger(other.gameObject);
    }

    private void TryStart(GameObject other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (hasTriggered) return;

        hasTriggered = true;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveOutAndBack());
    }

    private void ResetTrigger(GameObject other)
    {
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = false;

        if (!resetOnPlayerExit) return;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        SetPosition(startPosition);
    }

    private IEnumerator MoveOutAndBack()
    {
        Vector3 target = startPosition + moveOffset;
        Vector3 start = startPosition;

        yield return MoveTo(target);
        yield return MoveTo(start);

        moveCoroutine = null;
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        while (!IsAtPosition(destination))
        {
            Vector3 current = useLocalPosition ? moveTarget.localPosition : moveTarget.position;
            Vector3 next = Vector3.MoveTowards(current, destination, moveSpeed * Time.deltaTime);

            SetPosition(next);
            yield return null;
        }

        SetPosition(destination);
    }

    private bool IsAtPosition(Vector3 destination)
    {
        Vector3 current = useLocalPosition ? moveTarget.localPosition : moveTarget.position;
        return (current - destination).sqrMagnitude <= 0.0001f;
    }

    private void SetPosition(Vector3 value)
    {
        if (useLocalPosition)
        {
            moveTarget.localPosition = value;
        }
        else
        {
            moveTarget.position = value;
        }
    }
}
