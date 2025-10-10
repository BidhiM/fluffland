using UnityEngine;
using UnityEngine.SceneManagement;


public class mainmenusceneload : MonoBehaviour
{
    // Method to load the "SampleScene"
    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Method to load the "Shop" scene
    public void LoadShopScene()
    {
        SceneManager.LoadScene("shop");
    }

    // Method to load the "FaceUI" scene
    public void LoadFaceUIScene()
    {
        SceneManager.LoadScene("faceui");
    }
}