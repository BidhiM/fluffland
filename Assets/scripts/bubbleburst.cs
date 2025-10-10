using UnityEngine;
using System.Collections;

public class bubbleburst : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab;
    public Camera targetCamera; // Assign in the Inspector
    private camerafollowplayer dynamicCamera;

    private Vector3 originalCamPos;

    private void Start()
    {
        dynamicCamera = Camera.main.GetComponent<camerafollowplayer>();
        if (targetCamera == null)
        {
            Debug.LogWarning("Target camera is not assigned in the Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (particleSystemPrefab != null)
            {
                ParticleSystem spawnedParticles = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
                spawnedParticles.Play();
                Destroy(spawnedParticles.gameObject, spawnedParticles.main.duration);
            }

            if (dynamicCamera != null)
            {
                dynamicCamera.IncreaseFOVTemporarily(); // Increase FOV when particle spawns
            }

            if (targetCamera != null)
            {
                StartCoroutine(CameraShake(0.3f, 0.8f)); // Shake duration and intensity
            }

            Destroy(gameObject);
        }
    }

    private IEnumerator CameraShake(float duration, float magnitude)
    {
        float elapsed = 0f;
        originalCamPos = targetCamera.transform.position;

        while (elapsed < duration)
        {
            float xOffset = Random.Range(-1f, 1f) * magnitude;
            float yOffset = Random.Range(-1f, 1f) * magnitude;

            targetCamera.transform.position = originalCamPos + new Vector3(xOffset, yOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        targetCamera.transform.position = originalCamPos; // Reset position
    }
}