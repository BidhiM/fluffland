using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool isShaking = false;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.3f;
    private Vector3 originalPos;
    private Coroutine shakeCoroutine;
    public Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Assign main camera if not set
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0.0f;
        originalPos = mainCamera.transform.position; // Save position before shake

        while (elapsed < duration)
        {
            Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * magnitude;
            mainCamera.transform.position = originalPos + shakeOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalPos; // Reset camera position
        isShaking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShakeZone") && !isShaking)
        {
            if (shakeCoroutine != null) StopCoroutine(shakeCoroutine); // Ensure only one coroutine runs
            shakeCoroutine = StartCoroutine(Shake(shakeDuration, shakeMagnitude));
        }
        else if (other.CompareTag("StopShakeZone"))
        {
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                shakeCoroutine = null;
            }
            mainCamera.transform.position = originalPos; // Ensure reset
            isShaking = false;
        }
    }
}