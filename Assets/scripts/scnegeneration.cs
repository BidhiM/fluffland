using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SceneSpawnTrigger : MonoBehaviour
{
    [Tooltip("The empty GameObject that marks where the new scene should be placed.")]
    public Transform spawnPosition;
    private bool hasBeenTriggered = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player"))
        {
            hasBeenTriggered = true;
            Debug.Log($"[SceneSpawnTrigger] Player entered '{this.name}'. Requesting new scene...");
            if (spawnPosition == null)
            {
                Debug.LogError($"[SceneSpawnTrigger] 'spawnPosition' is NOT SET on '{this.name}'!", this);
                return;
            }
            if (SpawnManager.Instance == null)
            {
                Debug.LogError("[SceneSpawnTrigger] Cannot find SpawnManager! Is it in the scene?");
                return;
            }
            GameObject prefabToSpawn = SpawnManager.Instance.GetNextPrefabToSpawn();
            if (prefabToSpawn != null)
            {
                Instantiate(prefabToSpawn, spawnPosition.position, spawnPosition.rotation);
                Debug.Log($"[SceneSpawnTrigger] Instantiated '{prefabToSpawn.name}' at {spawnPosition.position}");
            }
            else
                Debug.LogError("[SceneSpawnTrigger] Manager returned NULL prefab. Check SpawnManager logs.");
        }
    }
}

