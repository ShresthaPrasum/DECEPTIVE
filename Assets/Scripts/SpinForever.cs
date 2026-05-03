using UnityEngine;

public class SpinForever : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float degreesPerSecond = 180f;
    [SerializeField] private Space rotationSpace = Space.Self;

    private void Update()
    {
        Vector3 axis = rotationAxis.normalized;
        if (axis == Vector3.zero) return;

        transform.Rotate(axis, degreesPerSecond * Time.deltaTime, rotationSpace);
    }
}
