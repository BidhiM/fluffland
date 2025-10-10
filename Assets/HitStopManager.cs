using System.Collections;
using UnityEngine;

public class HitstopManager : MonoBehaviour
{
    public static HitstopManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StopTime(float duration)
    {
        StartCoroutine(DoHitstop(duration));
    }

    private IEnumerator DoHitstop(float duration)
    {
        Time.timeScale = 0.4f; // Slow motion instead of full freeze
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}
