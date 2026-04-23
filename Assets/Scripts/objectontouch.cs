using Unity.VisualScripting;
using UnityEngine;

public class Objectontouch: MonoBehaviour
{
    public enum TouchAction
    {
        MoveObject,
        ToggleActiveState
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

    [SerializeField] private TouchAction action = TouchAction.MoveObject;

    [Header("Daya darwaja band karo")]

    [SerializeField] private Transform moveTarget;
    [SerializeField] private Vector3 moveOffset = new Vector3(0f,0f,0f);

    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private bool moveInLocalSpace = false;

    [SerializeField] private bool moveOnce = true;

    [SerializeField] private bool loopMovement= false;

    [SerializeField] private bool moveForeverAfterFirstTouch = true;
    

    [SerializeField] private GameObject targetObject;

    [SerializeField] private ActiveTouchMode activeTouchMode = ActiveTouchMode.SetState;

    [SerializeField] private bool setActiveOntouch = true;
    
    [SerializeField] private bool resetOnPlayerExit = false;

    private Vector3 startPosition;

    private Vector3 worldMoveOffset;

    private Vector3 positiveLoopPosition;

    private Vector3 negativeLoopPosition;

    private Vector3 targetPosition;

    private bool hasMoved;

    private bool isMoving;

    private bool movingToPositive;

    private bool foreverMovementStarted;

    private Transform resolvedMoveTarget;


    public void Awake()
    {
        if(targetObject == null)
        {
            targetObject = gameObject;
        }

        resolvedMoveTarget = moveTarget != null ? moveTarget : (targetObject != null ?  targetObject.transform:transform);

        startPosition = resolvedMoveTarget.position;

        worldMoveOffset = moveInLocalSpace ? resolvedMoveTarget.TransformVector(moveOffset): moveOffset;

        positiveLoopPosition = startPosition + worldMoveOffset;

        negativeLoopPosition = startPosition - worldMoveOffset;

        targetPosition = positiveLoopPosition;

        movingToPositive = true;

    }

    private void Update()
    {
        if(!isMoving)
        {
            return;
        }

        if(resolvedMoveTarget == null)
        {
            isMoving = false;
            return;
        }

        resolvedMoveTarget.position = Vector3.MoveTowards(resolvedMoveTarget.position, targetPosition, moveSpeed * Time.deltaTime);

        if((resolvedMoveTarget.position - targetPosition).sqrMagnitude <= 0.0001f)
        {
            resolvedMoveTarget.position = targetPosition;

            if(action == TouchAction.MoveObject && (loopMovement || foreverMovementStarted))
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

            if(moveOnce)
            {
                hasMoved = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleTouch(other.gameObject);
    }

    private void OTriggerEnter2D(Collider2D other)
    {
        HandleTouch(other.gameObject);
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
        if(!resetOnPlayerExit)
        {
            return;
        }

        if(moveForeverAfterFirstTouch && foreverMovementStarted)
        {
            return;
        }

        if(IsPlayer(other.gameObject))
        {
            ResetAction();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!resetOnPlayerExit)
        {
            return;
        }

        if(moveForeverAfterFirstTouch && foreverMovementStarted)
        {
            return;
        }
        if (IsPlayer(other.gameObject))
        {
            ResetAction();
        }
    }

    private void HandleTouch(GameObject touchedObject)
    {
        if (!IsPlayer(touchedObject))
        {
            return;
        }

        if(action == TouchAction.MoveObject)
        {
            if(resolvedMoveTarget  == null)
            {
                return;
            }

            worldMoveOffset = moveInLocalSpace ? resolvedMoveTarget.TransformVector(moveOffset) : moveOffset;

            positiveLoopPosition = startPosition + worldMoveOffset;

            negativeLoopPosition = startPosition - worldMoveOffset;

            if (moveForeverAfterFirstTouch)
            {
                foreverMovementStarted =true;
                targetPosition = positiveLoopPosition;

                movingToPositive= true;

                isMoving =true;
                return;
            }

            if(loopMovement)
            {
                targetPosition = positiveLoopPosition;

                movingToPositive= true;

                isMoving= true;
                return;
            }

            if(moveOnce && hasMoved)
            {
                return;
            }

            targetPosition = positiveLoopPosition;

            isMoving= true;
            return;
        }
    }

}