using UnityEngine;

public class knifeswing : MonoBehaviour
{
    public Transform pivot; // The pivot point
    public float amplitude = 45f; // Maximum swing angle in degrees
    public float speed = 2f; // Speed of the pendulum swing

    private float angle = 0f;

    void Update()
    {
        if (pivot == null)
        {
            Debug.LogWarning("Pivot is not assigned!");
            return;
        }

        angle = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = pivot.position + Quaternion.Euler(0, 0, angle) * Vector3.down * 2f;
    }
}
