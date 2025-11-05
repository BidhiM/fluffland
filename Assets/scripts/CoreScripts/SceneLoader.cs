using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject loadingUI;      // disabled by default in Hierarchy
    [SerializeField] Slider progressBar;        // optional
    [SerializeField] CanvasGroup loadingGroup;  // CanvasGroup on loading UI
    [SerializeField] CanvasGroup blackFader;    // CanvasGroup on pure black overlay
    [SerializeField] float fadeDuration = 0.3f;

    [Header("References")]
    [SerializeField] ScreenFader screenFader;   // your universal fader

    static SceneLoader instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Ensure a clean start (in case someone left things enabled in the editor)
        if (loadingGroup) loadingGroup.alpha = 0f;
        if (blackFader)  blackFader.alpha  = 0f;
        if (loadingUI)   loadingUI.SetActive(false);
        if (progressBar) progressBar.value = 0f;
    }

    public static void Load(string sceneName, bool useBlackFade = true)
    {
        if (instance == null)
        {
            Debug.LogError("SceneLoader not present in Core!");
            return;
        }
        instance.StartCoroutine(instance.LoadRoutine(sceneName, useBlackFade));
    }

    IEnumerator LoadRoutine(string sceneName, bool useBlackFade)
    {
        // Pick which CanvasGroup to fade this time.
        CanvasGroup fadeTarget = useBlackFade ? blackFader : loadingGroup;

        // Only show the loading UI if we’re actually using it for the fade.
        if (!useBlackFade && loadingUI) loadingUI.SetActive(true);

        // Fade out (either to black, or to the loading UI)
        if (screenFader && fadeTarget)
            yield return screenFader.FadeOut(fadeTarget, fadeDuration);

        // Start async load
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // Update progress only if the bar exists
        while (!op.isDone)
        {
            if (progressBar)
            {
                float progress = Mathf.Clamp01(op.progress / 0.9f);
                progressBar.value = progress;
            }

            if (op.progress >= 0.9f)
            {
                yield return new WaitForSecondsRealtime(0.15f);
                op.allowSceneActivation = true;
            }
            yield return null;
        }

        // Give one frame for scene activation to settle
        yield return null;

        // Fade back in (reveal the new scene)
        if (screenFader && fadeTarget)
            yield return screenFader.FadeIn(fadeTarget, fadeDuration);

        // Cleanup: hide the loading UI if it was used
        if (loadingUI && loadingUI.activeSelf) loadingUI.SetActive(false);
        if (progressBar) progressBar.value = 0f;

        // Ensure both groups are non-blocking after transitions
        if (loadingGroup) loadingGroup.blocksRaycasts = false;
        if (blackFader)  blackFader.blocksRaycasts  = false;
    }
}
