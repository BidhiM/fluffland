using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class parallaxcamerascript : MonoBehaviour
{
    private Camera cam;
    private BoxCollider2D camBox;
    private float sizeX, sizeY, ratio;

    void Start()
    {
        // Get the Camera component on the same GameObject
        cam = GetComponent<Camera>();
        camBox = GetComponent<BoxCollider2D>();

        if (cam == null || camBox == null)
        {
            Debug.LogError("Camera or BoxCollider2D is missing from the camera object!");
            return;
        }
    }

    void Update()
    {
        if (cam == null || camBox == null) return; // Safety check

        // Calculate the size based on the camera’s orthographic size and screen ratio
        sizeY = cam.orthographicSize * 2;
        ratio = (float)Screen.width / (float)Screen.height;
        sizeX = sizeY * ratio;

        // Update BoxCollider2D size based on camera size
        camBox.size = new Vector2(sizeX, sizeY);
    }
}