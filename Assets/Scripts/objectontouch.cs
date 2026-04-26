using UnityEngine;


public class ObjectMoveOnTrigger : MonoBehaviour
{
    public enum TouchAction
    {
        MoveObject,
        ToggleActiveState,
        ScaleObject 
    }

    public enum ActiveTouchMode
    {
        SetState,
        FlipState
    }

    [Header("Touch Filter")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool allowParentTagCheck = true;

    [Header("Action")]
    [SerializeField] private AudioClip triggerClip;
    [SerializeField] private TouchAction action = TouchAction.MoveObject;

    [Header("Kya bak rahe ho mader")]
    [SerializeField] private Transform moveTarget;
    [SerializeField] private Vector3 moveOffset = new Vector3(0f, 0f, 0f);
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool moveInLocalSpace = false;
    [SerializeField] private bool moveOnce = true;
    [SerializeField] private bool loopMovement = false;
    [SerializeField] private bool moveForeverAfterFirstTouch = true;

    [Header("Toggle MADER")]
    [SerializeField] private GameObject targetObject;
    [SerializeField] private ActiveTouchMode activeTouchMode = ActiveTouchMode.SetState;
    [SerializeField] private bool setActiveOnTouch = true;

    [Header("Scale Settings")]
    [Tooltip("How much to ADD to the object's original scale. E.g., (1,1,1) adds 1 to X, Y, and Z.")]
    [SerializeField] private Vector3 scaleAmountToAdd = new Vector3(1f, 1f, 1f); 
    [SerializeField] private float scaleRate = 3f;
    [SerializeField] private bool scaleOnce = true;

    [Header("Reset")]
    [SerializeField] private bool resetOnPlayerExit = false;

    private AudioSource triggerSoundSource;

    private Vector3 startPosition;
    private Vector3 startScale; 
    private Vector3 worldMoveOffset;
    private Vector3 positiveLoopPosition;
    private Vector3 negativeLoopPosition;
    private Vector3 targetPosition;
    
    private Vector3 calculatedTargetScale; 

    private bool hasMoved;
    private bool isMoving;
    private bool movingToPositive;
    private bool foreverMovementStarted;
    
    private bool isScaling; 
    private bool hasScaled; 

    private Transform resolvedMoveTarget;
    private bool hasPlayed = false;

    public void Awake()
    {
        triggerSoundSource = GetComponent<AudioSource>();
        if (triggerSoundSource == null)
        {
            triggerSoundSource = gameObject.AddComponent<AudioSource>();
        }

        if (targetObject == null)
        {
            targetObject = gameObject;
        }

        resolvedMoveTarget = moveTarget != null ? moveTarget : (targetObject != null ? targetObject.transform : transform);

        
        startPosition = resolvedMoveTarget.position;
        worldMoveOffset = moveInLocalSpace ? resolvedMoveTarget.TransformVector(moveOffset) : moveOffset;
        positiveLoopPosition = startPosition + worldMoveOffset;
        negativeLoopPosition = startPosition - worldMoveOffset;
        targetPosition = positiveLoopPosition;
        movingToPositive = true;

        
        startScale = resolvedMoveTarget.localScale;
    }

    private void Update()
    {
        if (resolvedMoveTarget == null) return;

        
        if (action == TouchAction.MoveObject && isMoving)
        {
            resolvedMoveTarget.position = Vector3.MoveTowards(resolvedMoveTarget.position, targetPosition, moveSpeed * Time.deltaTime);

            if ((resolvedMoveTarget.position - targetPosition).sqrMagnitude <= 0.0001f)
            {
                resolvedMoveTarget.position = targetPosition;

                if (loopMovement || foreverMovementStarted)
                {
                    if (movingToPositive)
                    {
                        targetPosition = negativeLoopPosition;
                        movingToPositive = false;
                    }
                    else
                    {
                        targetPosition = positiveLoopPosition;
                        movingToPositive = true;
                    }
                    isMoving = true;
                    return;
                }

                isMoving = false;

                if (moveOnce)
                {
                    hasMoved = true;
                }
            }
        }
        
        else if (action == TouchAction.ScaleObject && isScaling)
        {
            resolvedMoveTarget.localScale = Vector3.MoveTowards(resolvedMoveTarget.localScale, calculatedTargetScale, scaleRate * Time.deltaTime);

            if ((resolvedMoveTarget.localScale - calculatedTargetScale).sqrMagnitude <= 0.0001f)
            {
                resolvedMoveTarget.localScale = calculatedTargetScale;
                isScaling = false;

                if (scaleOnce)
                {
                    hasScaled = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleTouch(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleTouch(other.gameObject);

        if (hasPlayed == false)
        {
            triggerSoundSource.PlayOneShot(triggerClip);
            hasPlayed = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleTouch(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleTouch(collision.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!resetOnPlayerExit) return;
        if (moveForeverAfterFirstTouch && foreverMovementStarted) return;

        if (IsPlayer(other.gameObject))
        {
            ResetAction();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!resetOnPlayerExit) return;
        if (moveForeverAfterFirstTouch && foreverMovementStarted) return;

        if (IsPlayer(other.gameObject))
        {
            ResetAction();
        }
    }

    private void HandleTouch(GameObject touchedObject)
    {
        if (!IsPlayer(touchedObject)) return;

        
        if (action == TouchAction.MoveObject)
        {
            if (resolvedMoveTarget == null) return;

            worldMoveOffset = moveInLocalSpace ? resolvedMoveTarget.TransformVector(moveOffset) : moveOffset;
            positiveLoopPosition = startPosition + worldMoveOffset;
            negativeLoopPosition = startPosition - worldMoveOffset;

            if (moveForeverAfterFirstTouch)
            {
                foreverMovementStarted = true;
                targetPosition = positiveLoopPosition;
                movingToPositive = true;
                isMoving = true;
                return;
            }

            if (loopMovement)
            {
                targetPosition = positiveLoopPosition;
                movingToPositive = true;
                isMoving = true;
                return;
            }

            if (moveOnce && hasMoved) return;

            targetPosition = positiveLoopPosition;
            isMoving = true;
            return;
        }

        
        if (action == TouchAction.ScaleObject)
        {
            if (resolvedMoveTarget == null) return;
            if (scaleOnce && hasScaled) return;

            
            calculatedTargetScale = startScale + scaleAmountToAdd;

            isScaling = true;
            return;
        }

        
        if (action == TouchAction.ToggleActiveState && targetObject != null)
        {
            if (activeTouchMode == ActiveTouchMode.FlipState)
            {
                targetObject.SetActive(!targetObject.activeSelf);
            }
            else
            {
                targetObject.SetActive(setActiveOnTouch);
            }
        }
    }

    private bool IsPlayer(GameObject candidate)
    {
        if (candidate == null) return false;
        if (candidate.CompareTag(playerTag)) return true;
        if (!allowParentTagCheck) return false;

        Transform current = candidate.transform.parent;
        while (current != null)
        {
            if (current.CompareTag(playerTag)) return true;
            current = current.parent;
        }

        return false;
    }

    private void ResetAction()
    {
        if (action == TouchAction.MoveObject)
        {
            if (resolvedMoveTarget != null)
            {
                resolvedMoveTarget.position = startPosition;
            }
            targetPosition = positiveLoopPosition;
            movingToPositive = true;
            foreverMovementStarted = false;
            isMoving = false;
        }
        else if (action == TouchAction.ScaleObject)
        {
            if (resolvedMoveTarget != null)
            {
                resolvedMoveTarget.localScale = startScale;
            }
            isScaling = false;
            hasScaled = false;
        }
    }
}        