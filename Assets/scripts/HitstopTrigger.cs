using UnityEngine;

public class HitstopTrigger : MonoBehaviour
{
    public float hitstopDuration = 0.3f; // Set this per object

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHit>() != null) // Player must have PlayerHit script
        {
            HitstopManager.Instance.StopTime(hitstopDuration);
        }
    }
}