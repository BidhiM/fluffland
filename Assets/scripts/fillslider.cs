using UnityEngine;
using UnityEngine.UI;

public class fillslider : MonoBehaviour
{
    public Slider slider; // Reference to the UI Slider
    public float fillSpeed = 1f; // Speed of filling (editable in Inspector)

    private bool isFilling = false; // Track if filling has started
    public float targetValue = 1f; // Target value (1 = full)

    void Update()
    {
        // Detect first touch or mouse click
        if (Input.GetMouseButtonDown(0) && !isFilling)
        {
            isFilling = true; // Start filling
        }

        // Gradually fill the slider if it has started
        if (isFilling && slider.value < targetValue)
        {
            slider.value += fillSpeed * Time.deltaTime;
        }
    }
}