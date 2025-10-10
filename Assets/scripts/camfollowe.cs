using UnityEngine;

public class camfollowe : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = player.position + offset;
        transform.LookAt(player);

    }
}
