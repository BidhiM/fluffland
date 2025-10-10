using UnityEngine;

public class raycastplayer : MonoBehaviour
{
    public float rayDistance = 5f; // Maximum distance for the raycast
    public LayerMask targetLayer; // LayerMask for raycasting
    public GameObject aboveObject; // GameObject to track the "above" hit position
    public GameObject belowObject; // GameObject to track the "below" hit position

    void Update()
    {
        Vector2 origin = transform.position; // Player's position

        // Cast ray upward and update aboveObject position
        RaycastHit2D hitAbove = Physics2D.Raycast(origin, Vector2.up, rayDistance, targetLayer);
        if (hitAbove.collider != null && hitAbove.collider.CompareTag("abovegr"))
        {
            aboveObject.transform.position = hitAbove.point; // Continuously update aboveObject position
        }
        else
        {
            Debug.Log("No object above detected within range.");
        }

        // Cast ray downward and update belowObject position
        RaycastHit2D hitBelow = Physics2D.Raycast(origin, Vector2.down, rayDistance, targetLayer);
        if (hitBelow.collider != null && hitBelow.collider.CompareTag("belowgr"))
        {
            belowObject.transform.position = hitBelow.point; // Continuously update belowObject position
        }
        else
        {
            Debug.Log("No object below detected within range.");
        }

        // Visualize the rays in the scene view for debugging
        Debug.DrawRay(origin, Vector2.up * rayDistance, Color.green);
        Debug.DrawRay(origin, Vector2.down * rayDistance, Color.blue);
    }
}