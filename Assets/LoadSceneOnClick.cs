using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    // Name of the scene to load
    public string sceneName = "SampleScene";

    // Function called when the button is clicked
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
