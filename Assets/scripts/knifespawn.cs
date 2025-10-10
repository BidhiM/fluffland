using UnityEngine;

public class knifespawn : MonoBehaviour
{
    public GameObject knifePrefab; // Assign the knife prefab in the Inspector
    public int knifeCount = 5; // Number of knives to spawn
    public Vector2 spawnAreaMin = new Vector2(-5f, -3f); // Min bounds of spawn area
    public Vector2 spawnAreaMax = new Vector2(5f, 3f); // Max bounds of spawn area
    public float minDistance = 2f; // Minimum distance between knives

    private bool spawningActive = false;
    private GameObject[] spawnedKnives;

    void Start()
    {
        spawnedKnives = new GameObject[knifeCount];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enter") && !spawningActive)
        {
            spawningActive = true;
            SpawnKnives();
        }
        else if (other.CompareTag("Exit") && spawningActive)
        {
            spawningActive = false;
        }
    }

    void SpawnKnives()
    {
        int spawned = 0;
        while (spawned < knifeCount)
        {
            Vector2 spawnPos = GetValidSpawnPosition();
            spawnedKnives[spawned] = Instantiate(knifePrefab, spawnPos, Quaternion.identity);
            spawned++;
        }
    }

    Vector2 GetValidSpawnPosition()
    {
        Vector2 pos;
        bool valid;
        int attempts = 0;

        do
        {
            pos = new Vector2(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y));
            valid = true;

            foreach (GameObject knife in spawnedKnives)
            {
                if (knife != null && Vector2.Distance(pos, knife.transform.position) < minDistance)
                {
                    valid = false;
                    break;
                }
            }
            attempts++;
        } while (!valid && attempts < 10); // Avoid infinite loops

        return pos;
    }

}