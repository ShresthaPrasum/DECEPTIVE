using UnityEngine;

public class SpinForever : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    public float degreesPerSecond = 180f;
    [SerializeField] private Space rotationSpace = Space.Self;

    [Header("Move")]
    [SerializeField] private bool enableMovement = false;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool useLocalPosition = false;

    private Vector3 startPosition;
    private bool movingToPositive = true;
    private float pauseTimer;

    private void Awake()
    {
        startPosition = useLocalPosition ? transform.localPosition : transform.position;
    }

    private void Update()
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        Vector3 axis = rotationAxis.normalized;
        if (axis == Vector3.zero) return;

        transform.Rotate(axis, degreesPerSecond * Time.deltaTime, rotationSpace);

        if (enableMovement)
        {
            Vector3 current = useLocalPosition ? transform.localPosition : transform.position;
            Vector3 positive = startPosition + (Vector3.right * moveDistance);
            Vector3 negative = startPosition - (Vector3.right * moveDistance);
            Vector3 target = movingToPositive ? positive : negative;

            Vector3 next = Vector3.MoveTowards(current, target, moveSpeed * Time.deltaTime);
            SetPosition(next);

            if ((next - target).sqrMagnitude <= 0.0001f)
            {
                movingToPositive = !movingToPositive;
            }
        }
    }

    public void PauseForSeconds(float seconds)
    {
        if (seconds <= 0f) return;
        pauseTimer = Mathf.Max(pauseTimer, seconds);
    }

    private void SetPosition(Vector3 value)
    {
        if (useLocalPosition)
        {
            transform.localPosition = value;
        }
        else
        {
            transform.position = value;
        }
    }
}
