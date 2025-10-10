using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Vector3 startPosition;
    private float startCameraX;
    private Transform cameraTransform;
    public float parallaxFactor = 0.05f; // Adjust for depth effect

    void Start()
    {
        cameraTransform = Camera.main.transform;
        startPosition = transform.position;  // Store the exact original position
        startCameraX = cameraTransform.position.x; // Store initial camera position
    }

    void LateUpdate()
    {
        // Only calculate the difference from the starting camera position
        float cameraOffsetX = cameraTransform.position.x - startCameraX;

        // Apply a purely visual offset without actually moving the object
        transform.position = startPosition + Vector3.right * (cameraOffsetX * parallaxFactor);
    }
}