using UnityEngine;
using UnityEngine.SceneManagement;

public class respawning : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SceneLoader.Load("respawningobstacle");
        }
    }

    public void Retry()
    {
        SceneLoader.Load("SampleScene");
    }

    public void LoadMainMenu()
    {
        SceneLoader.Load("MAINMENUNEW");
    }

}