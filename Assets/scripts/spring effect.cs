using UnityEngine;

public class springeffect : MonoBehaviour
{
    [Header("Spring Motion Settings")]
    public float amplitude = 1f; // Maximum height of the motion
    public float frequency = 1f; // Speed of the up and down motion

    private Vector3 startPosition;

    void Start()
    {
        // Save the initial position of the GameObject
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new position using a sine wave
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, startPosition.y + yOffset, startPosition.z);
    }
}