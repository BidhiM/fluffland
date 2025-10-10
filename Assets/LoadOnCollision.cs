using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnCollision : MonoBehaviour
{
    public string sceneName = "MAINMENUNEW";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player touches the collider
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

