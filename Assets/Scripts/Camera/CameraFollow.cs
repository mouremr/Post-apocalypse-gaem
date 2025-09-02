using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Reference to the player's transform
    public float smoothing = 5f;      // How quickly the camera catches up
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Camera position offset
    
    void FixedUpdate()
    {
        if (target != null)
        {
            // Calculate the desired position
            Vector3 targetPosition = target.position + offset;
            
            // Smoothly move the camera towards that position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }
    }
}