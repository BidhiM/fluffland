using UnityEngine;

public class crownthreadmove : MonoBehaviour
{
    public float speed = 3f;  // Speed of movement
    public float range = 3f;  // Maximum distance from the starting position

    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;
    }

    void Update()
    {
        float movement = speed * Time.deltaTime * (movingRight ? 1 : -1);
        transform.position += new Vector3(movement, 0, 0);

        // Swing effect (optional)
        float swingAngle = Mathf.Sin(Time.time * speed) * 10f; // 10 degrees max swing
        transform.rotation = Quaternion.Euler(0, 0, swingAngle);

        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            movingRight = !movingRight;
        }
    }
}
