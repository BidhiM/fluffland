using UnityEngine;

public class instantiatejugnu : MonoBehaviour
{
    [Header("Prefab Settings")]
    [Tooltip("The prefab to instantiate when the player enters the trigger.")]
    public GameObject prefabToInstantiate;

    [Header("Spawn Position")]
    [Tooltip("The position where the prefab will be instantiated. Leave empty to spawn at this object's position.")]
    public Transform spawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            Debug.Log("COLLIDED");
            // Ensure the prefab is assigned
            if (prefabToInstantiate != null)
            {
                // Determine the spawn position
                Vector3 position = spawnPosition != null ? spawnPosition.position : transform.position;

                // Instantiate the prefab at the spawn position
                Instantiate(prefabToInstantiate, position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Prefab to instantiate is not assigned in the Inspector.");
            }
        }
    }
}