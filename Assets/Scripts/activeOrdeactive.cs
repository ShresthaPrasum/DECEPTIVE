using UnityEngine;

public class activeOrdeactive : MonoBehaviour
{
    [Header("TargetObject")]
    public GameObject targetObject;

    [SerializeField] private bool activate;

    private bool isActive;

    private void Awake()
    {
        isActive = targetObject.activeSelf;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!activate)
            {
                targetObject.SetActive(false);
            }
            else
            {
                targetObject.SetActive(false);
            }
        }
        
    }
}