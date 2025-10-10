using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class parallaxlayer : MonoBehaviour
{
    private Vector2 currentPosition, lastPosition, positionDifference;
    private bool parallaxNow;
    public float speed;

    void LateUpdate()
    {
        if (parallaxNow)
        {
            DetectCameraMovement();
            // Move the object based on camera movement
            transform.Translate(new Vector3(positionDifference.x, positionDifference.y, 0) * speed * Time.deltaTime, Space.World);
        }
    }

    void DetectCameraMovement()
    {
        // Ensure the camera reference is correct and check if camera exists
        if (Camera.main == null) return;

        currentPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        if (currentPosition == lastPosition)
        {
            positionDifference = Vector2.zero;
        }
        else
        {
            positionDifference = currentPosition - lastPosition;
        }

        lastPosition = currentPosition;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("MainCamera"))
        {
            lastPosition = currentPosition = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            parallaxNow = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("MainCamera"))
        {
            parallaxNow = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("MainCamera"))
        {
            positionDifference = Vector2.zero;
            parallaxNow = false;
        }
    }
}