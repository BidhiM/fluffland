using UnityEngine;

public class dragon : MonoBehaviour
{
    public Animator animator; // Assign the Animator in the Inspector
    public Camera mainCamera; // Assign the main camera in the Inspector
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public GameObject drago;

    private Vector3 originalCameraPosition;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        originalCameraPosition = mainCamera.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("collidragon"))
        {
            drago.SetActive(true);
          
            StartCoroutine(ShakeCamera());
        }
    }

    private System.Collections.IEnumerator ShakeCamera()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalCameraPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            mainCamera.transform.position = new Vector3(randomPoint.x, randomPoint.y, originalCameraPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = originalCameraPosition;
    }
}