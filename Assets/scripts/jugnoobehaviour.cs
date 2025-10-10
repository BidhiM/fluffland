using UnityEngine;

public class jugnoobehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed at which the object moves toward the player.")]
    public float moveSpeed = 5f;

    [Header("Wait Duration")]
    [Tooltip("Time to wait near the player before destroying this object.")]
    public float waitDur = 1f;

    private Transform playerTransform;
    private bool isNearPlayer = false;

    private void Start()
    {
        // Find the player object by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No GameObject with the 'Player' tag was found in the scene.");
        }
    }

    private void Update()
    {
        // If the player is found and we are not near the player yet
        if (playerTransform != null && !isNearPlayer)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            // Check if the object is close to the player
            if (Vector2.Distance(transform.position, playerTransform.position) < 0.1f)
            {
                isNearPlayer = true; // Object has reached the player
                StartCoroutine(WaitAndDestroy());
            }
        }
    }

    private System.Collections.IEnumerator WaitAndDestroy()
    {
        // Wait near the player for the specified duration
        yield return new WaitForSeconds(waitDur);

        // Destroy this GameObject
        Destroy(gameObject);
    }
}