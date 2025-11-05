using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class lighttransition : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D[] spotLights;
    public Light2D[] freeformLights;
    public Volume globalVolume;
    public float fadeDuration = 2f;

    private Vignette vignette;
    private Bloom bloom;

    void Start()
    {
        if (globalLight == null)
            globalLight = FindFirstObjectByType<Light2D>(); // Auto-find global light

        if (globalVolume.profile.TryGet(out vignette))
            vignette.intensity.value = 0.5f; // Start with strong vignette

        if (globalVolume.profile.TryGet(out bloom))
            bloom.intensity.value = 0f; // Start with no bloom

        StartCoroutine(FadeInLights());
    }

    IEnumerator FadeInLights()
    {
        float elapsedTime = 0f;
        float startIntensity = 0f;
        float targetIntensity = globalLight.intensity;

        // Set initial intensities
        globalLight.intensity = startIntensity;
        foreach (var light in spotLights) light.intensity = startIntensity;
        foreach (var light in freeformLights) light.intensity = startIntensity;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / fadeDuration;

            // Light Fades
            globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, progress);
            foreach (var light in spotLights) light.intensity = Mathf.Lerp(startIntensity, targetIntensity, progress);
            foreach (var light in freeformLights) light.intensity = Mathf.Lerp(startIntensity, targetIntensity, progress);

            // Vignette Fade Out
            vignette.intensity.value = Mathf.Lerp(0.5f, 0f, progress);

            // Bloom Fade In
            bloom.intensity.value = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        // Ensure final values are set
        globalLight.intensity = targetIntensity;
        foreach (var light in spotLights) light.intensity = targetIntensity;
        foreach (var light in freeformLights) light.intensity = targetIntensity;
        vignette.intensity.value = 0f;
        bloom.intensity.value = 1f;
    }
}