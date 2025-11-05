using UnityEngine;
public class Core : MonoBehaviour
{
    [SerializeField] string firstSceneToLoad = "MAINMENUNEW";
    static bool _initialized;

    void Awake()
    {
        if (_initialized)
        {
            Destroy(gameObject);
            return;
        }

        _initialized = true;
        DontDestroyOnLoad(gameObject);
        SceneLoader.Load(firstSceneToLoad, useBlackFade: false);
    }
}
