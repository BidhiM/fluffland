using UnityEngine;

public class cameramovement : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;               // The player to follow

    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 0.125f;     // Speed of the camera's smoothing
    [SerializeField] private Vector3 offset;                 // Offset to maintain relative to the player
    [SerializeField] private float horizontalFollowRange = 1f; // Horizontal movement range before the camera moves

    [Header("Bounds")]
    [SerializeField] private bool enableBounds = false;      // Toggle bounds for the camera
    [SerializeField] private Vector2 minBounds;              // Minimum camera position (x, y)
    [SerializeField] private Vector2 maxBounds;              // Maximum camera position (x, y)

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Target not assigned to CameraFollow script!");
            return;
        }

        FollowTarget();
    }

    private void FollowTarget()
    {
        // Compute the desired camera position
        Vector3 desiredPosition = target.position + offset;
        Vector3 currentPosition = transform.position;

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(currentPosition, desiredPosition, smoothSpeed);

        // Handle horizontal follow range
        if (Mathf.Abs(target.position.x - currentPosition.x) > horizontalFollowRange)
        {
            smoothedPosition.x = Mathf.Lerp(currentPosition.x, desiredPosition.x, smoothSpeed);
        }

        // Apply camera bounds if enabled
        if (enableBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }

        // Keep the z-position constant for 2D games
        smoothedPosition.z = currentPosition.z;

        // Update the camera's position
        transform.position = smoothedPosition;
    }
}