using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;  
        transform.position = mousePosition;
    }
}
