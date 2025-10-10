using UnityEngine;
using System.Collections;

public class flowerboostscript : MonoBehaviour
{
    [Header("Player Settings")]
    // Reference to the player object
    public GameObject player;

    [Header("Spawn Settings")]
    // Prefab of the small circle to instantiate
    public GameObject circlePrefab;

    // Speed at which the circle moves toward the player
    public float moveSpeed = 5.0f;

    [Header("Vanish Settings")]
    // Duration for which the object remains attached before vanishing
    public float vanishDelay = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object colliding is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Instantiate a small circle at this object's position
            GameObject spawnedCircle = Instantiate(circlePrefab, transform.position, Quaternion.identity);

            // Start the process of moving the circle toward the player and attaching it
            //  StartCoroutine(MoveAndAttach(spawnedCircle));
        }
    }
}

