using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class scenechange : MonoBehaviour
{
    public string sceneName; // Scene to load (set in Inspector)
    public SpriteRenderer fadeSprite; // Sprite that will fade in
    public float transitionSpeed = 1f; // Speed of the fade effect
    private bool isFading = false;
    private float alpha = 0f;
    private Light2D[] sceneLights;
    private Light2D globalLight;
    private Volume globalVolume;
    private Bloom bloom;
    private Vignette vignette;
    private ShadowCaster2D playerShadowCaster;

    private void Start()
    {
        if (fadeSprite != null)
        {
            // Ensure sprite starts fully transparent
            Color color = fadeSprite.color;
            color.a = 0f;
            fadeSprite.color = color;
        }
        else
        {
            Debug.LogError("[SceneChange] No fade sprite assigned!");
        }

        // Get all lights in the scene
        sceneLights = FindObjectsOfType<Light2D>();
        globalLight = FindObjectOfType<Light2D>(); // Assuming one global light exists

        // Get global volume and post-processing effects
        globalVolume = FindObjectOfType<Volume>();
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out bloom);
            globalVolume.profile.TryGet(out vignette);
        }

        // Get player's ShadowCaster2D
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerShadowCaster = player.GetComponent<ShadowCaster2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            Debug.Log("[SceneChange] Player entered. Starting fade transition.");
            isFading = true;
            if (playerShadowCaster != null)
            {
                playerShadowCaster.enabled = false;
            }
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        while (alpha < 1f)
        {
            alpha += transitionSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            // Fade sprite
            if (fadeSprite != null)
            {
                Color color = fadeSprite.color;
                color.a = alpha;
                fadeSprite.color = color;
            }

            // Fade lights
            foreach (Light2D light in sceneLights)
            {
                light.intensity = Mathf.Lerp(light.intensity, 0f, transitionSpeed * Time.deltaTime);
            }

            if (globalLight != null)
            {
                globalLight.intensity = Mathf.Lerp(globalLight.intensity, 0f, transitionSpeed * Time.deltaTime);
            }

            // Fade post-processing effects
            if (bloom != null)
            {
                bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, 0f, transitionSpeed * Time.deltaTime);
            }

            if (vignette != null)
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 1f, transitionSpeed * Time.deltaTime);
            }

            yield return null;
        }

        Debug.Log("[SceneChange] Fade complete. Changing scene.");
        SceneManager.LoadScene(sceneName);
    }
}