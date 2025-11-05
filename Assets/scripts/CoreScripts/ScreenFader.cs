using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [Header("Default Settings")]
    [SerializeField] CanvasGroup defaultCanvasGroup;   // fallback if no CanvasGroup is passed
    [SerializeField] float duration = 0.5f;
    [SerializeField] bool fadeInOnStart = true;

    void Awake()
    {
        // 1. Try to get the CanvasGroup
        if (!defaultCanvasGroup)
            defaultCanvasGroup = GetComponent<CanvasGroup>();

        // 2. Add a null check for safety
        if (!defaultCanvasGroup)
        {
            Debug.LogWarning("ScreenFader: No default CanvasGroup found on " + gameObject.name + ". FadeInOnStart will not work.");
            fadeInOnStart = false; // Disable to prevent errors
            return;
        }

        // 3. (THE FIX) If we intend to fade in, force the alpha to 1 immediately.
        // This prevents the "snap" and ensures the state is correct.
        if (fadeInOnStart)
        {
            defaultCanvasGroup.alpha = 1f;
        }
    }

    private IEnumerator Fade(CanvasGroup target, float from, float to, float d)
    {
        if (!target) yield break;

        float t = 0f;
        target.blocksRaycasts = true; // block clicks during fades

        while (t < d)
        {
            target.alpha = Mathf.Lerp(from, to, t / d);
            yield return null;
            t += Time.unscaledDeltaTime; // unaffected by timescale
        }

        target.alpha = to;
        target.blocksRaycasts = to > 0.001f;
    }

    public IEnumerator FadeOut(CanvasGroup target = null, float d = -1f)
    {
        CanvasGroup cg = target ? target : defaultCanvasGroup;
        yield return Fade(cg, 0f, 1f, d > 0 ? d : duration);
    }

    public IEnumerator FadeIn(CanvasGroup target = null, float d = -1f)
    {
        CanvasGroup cg = target ? target : defaultCanvasGroup;
        yield return Fade(cg, 1f, 0f, d > 0 ? d : duration);
    }

    void Start()
    {
        if (fadeInOnStart && defaultCanvasGroup)
            StartCoroutine(FadeIn(defaultCanvasGroup, duration));
    }
}
