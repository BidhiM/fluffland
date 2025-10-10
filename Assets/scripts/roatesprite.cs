using UnityEngine;

public class roatesprite : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation in degrees per second

    void Update()
    {
        // Rotate the sprite around the Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}