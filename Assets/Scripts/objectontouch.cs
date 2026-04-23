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

    [SerializeField] private Vector3 moveOffset = new Vector3(0f,0f,0f);

    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private bool moveInLocalSpace = false;
}