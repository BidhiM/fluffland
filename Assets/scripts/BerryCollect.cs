using UnityEngine;

public class BerryCollect : MonoBehaviour
{
    public float detectionRadius = 5f; // Minimum radius to detect player
    public float moveSpeed = 5f;       // Speed at which berry moves toward player

    private Transform player;
    private bool isMovingToPlayer = false;

    void Start()
    {
        // Find the player in the scene by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Check if player is within detection radius
        if (distance <= detectionRadius)
        {
            isMovingToPlayer = true;
        }

        // Move towards player if activated
        if (isMovingToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Check if berry reached the player
            if (distance <= 0.1f)
            {
                CollectBerry();
            }
        }
    }

    void CollectBerry()
    {
        // Increase berry count
        BerryManager.Instance.AddBerry();

        // Destroy the berry
        Destroy(gameObject);
    }

    // Optional: visualize detection radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

