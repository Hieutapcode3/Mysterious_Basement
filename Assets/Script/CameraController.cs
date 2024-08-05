using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Transform playerTransform;  
    public float mouseInfluence = 0.5f; 

    private Vector3 targetPosition;

    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; 

        targetPosition = playerTransform.position + (mouseWorldPosition - playerTransform.position) * mouseInfluence;

        transform.position = targetPosition;
    }
}
