using UnityEngine;

public class camfollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform player; // Reference to the player object

    [Header("Camera Offset")]
    public Vector3 offset = new Vector3(0, 5, -10); // Offset from the player's position

    [Header("Smoothness Settings")]
    public float followSpeed = 5f; // Speed at which the camera follows the player

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the target position based on the player's position and offset
            Vector3 targetPosition = player.position + offset;

            // Smoothly move the camera to the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
