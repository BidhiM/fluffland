using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ScnGeneration : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] scenes;                // Prefabs to be spawned (non-transition)
    public GameObject transitionPrefab;        // Transition / tunnel prefab (set in Inspector)
    public Transform nextPrefabSpawnPosition;  // Spawn position (set in Inspector)

    [Header("Debugging")]
    [Tooltip("If checked, the spawner will repeatedly instantiate only the 'debugPrefab' below, ignoring the normal cycle.")]
    public bool forceSpecificPrefab = false;
    [Tooltip("The specific prefab to spawn when 'forceSpecificPrefab' is enabled.")]
    public GameObject debugPrefab;

    // Shared across all Spawner instances in the scene
    private static int spawnCount = 0;

    // Shared set of prefab indices that are still left to spawn in the current cycle.
    // We store indices into the 'scenes' array.
    private static List<int> remainingPrefabIndices;
    private static int sharedScenesLength = 0; // to detect if scenes length changed (re-init if needed)

    private void Start()
    {
        if (scenes == null || scenes.Length == 0)
        {
            Debug.LogWarning("[Spawner] No prefabs assigned to the scenes array!");
            return;
        }

        if (transitionPrefab == null)
        {
            Debug.LogWarning("[Spawner] No transition prefab assigned!");
            return;
        }

        if (nextPrefabSpawnPosition == null)
        {
            Debug.LogWarning("[Spawner] NextPrefabSpawnPosition is not assigned!");
        }

        // Initialize or re-initialize the shared remaining indices if nothing exists
        // or if the scenes length changed (to avoid index-out-of-range when different spawners use different arrays).
        if (remainingPrefabIndices == null || sharedScenesLength != scenes.Length)
        {
            InitializeSharedPool();
        }
    }

    private void InitializeSharedPool()
    {
        remainingPrefabIndices = new List<int>(scenes.Length);
        for (int i = 0; i < scenes.Length; i++)
        {
            remainingPrefabIndices.Add(i);
        }

        sharedScenesLength = scenes.Length;
        Debug.Log($"[Spawner] Initialized shared pool with {sharedScenesLength} prefab indices.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[Spawner] Player triggered the spawner.");
            SpawnPrefab();
        }
    }

    private void SpawnPrefab()
    {
        if (nextPrefabSpawnPosition == null)
        {
            Debug.LogError("[Spawner] NextPrefabSpawnPosition is not assigned!");
            return;
        }

        GameObject selectedPrefab = null;

        // --- DEBUG MODE LOGIC ---
        // If debug mode is on and a prefab is assigned, spawn it and skip the normal logic.
        if (forceSpecificPrefab)
        {
            if (debugPrefab != null)
            {
                selectedPrefab = Instantiate(debugPrefab);
                Debug.Log($"[Spawner] DEBUG MODE: Forcing spawn of '{debugPrefab.name}'");
            }
            else
            {
                Debug.LogError("[Spawner] DEBUG MODE is on, but no 'debugPrefab' has been assigned in the Inspector!");
                return;
            }
        }
        // --- NORMAL SPAWN LOGIC ---
        else
        {
            if (scenes == null || scenes.Length == 0)
            {
                Debug.LogError("[Spawner] No prefabs available to spawn.");
                return;
            }

            if (transitionPrefab == null)
            {
                Debug.LogError("[Spawner] Transition prefab not assigned.");
                return;
            }

            // If the remaining pool is empty, it means we've already spawned every prefab once in the cycle.
            // The *next* spawn should be the transition/tunnel prefab. After spawning it, refill the pool.
            if (remainingPrefabIndices == null || remainingPrefabIndices.Count == 0)
            {
                // Safety: if somehow remainingPrefabIndices is null, initialize it now
                if (remainingPrefabIndices == null)
                    InitializeSharedPool();

                // Spawn the transition prefab
                selectedPrefab = Instantiate(transitionPrefab);
                Debug.Log($"[Spawner] Spawned TRANSITION prefab at {nextPrefabSpawnPosition.position}");

                // Refill the pool for the next cycle
                InitializeSharedPool();
            }
            else
            {
                // Pick a random index from remainingPrefabIndices and remove it from the pool
                int randomPoolIndex = Random.Range(0, remainingPrefabIndices.Count);
                int prefabArrayIndex = remainingPrefabIndices[randomPoolIndex];
                remainingPrefabIndices.RemoveAt(randomPoolIndex);

                // Instantiate the selected prefab
                selectedPrefab = Instantiate(scenes[prefabArrayIndex]);
                Debug.Log($"[Spawner] Spawned {selectedPrefab.name} (prefabIndex={prefabArrayIndex}) at {nextPrefabSpawnPosition.position}");

                // Note: the transition will spawn on the next trigger once this removal emptied the pool
                if (remainingPrefabIndices.Count == 0)
                {
                    Debug.Log("[Spawner] All prefabs spawned once this cycle. Next spawn will be the TRANSITION prefab.");
                }
                else
                {
                    Debug.Log($"[Spawner] {remainingPrefabIndices.Count} prefabs left in this cycle.");
                }
            }
        }

        // Place the instantiated prefab at the spawn position
        if (selectedPrefab != null)
        {
            selectedPrefab.transform.position = nextPrefabSpawnPosition.position;
            selectedPrefab.transform.rotation = nextPrefabSpawnPosition.rotation;

            spawnCount++;
            Debug.Log($"[Spawner] Total Spawns: {spawnCount}");
        }
    }
}
