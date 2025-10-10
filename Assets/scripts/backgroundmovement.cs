using UnityEngine;

public class backgrndmovemnt : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private Transform player; // Reference to the player GameObject

    private float backgroundY; // Cached vertical position of the background
    private float backgroundZ; // Cached depth position of the background

    private void Start()
    {
        // Store the initial vertical and Z positions of the background
        backgroundY = transform.position.y;
        backgroundZ = transform.position.z;

        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned to the BackgroundFollowPlayer script.");
        }
    }

    private void LateUpdate()
    {
        if (player == null) return;

        // Update only the X position to follow the player, keeping Y and Z constant
        transform.position = new Vector3(player.position.x, backgroundY, backgroundZ);
    }
}
