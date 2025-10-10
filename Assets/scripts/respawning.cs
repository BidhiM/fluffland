using UnityEngine;
using UnityEngine.SceneManagement;

public class respawning : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene("respawningobstacle");
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MAINMENUNEW");
    }
}