using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // --- Inspector Fields ---
    [Tooltip("The Transform of the object to track (usually the player).")]
    public Transform playerTransform;

    [Tooltip("The TextMeshPro UI element to display the score.")]
    public TextMeshProUGUI scoreText;

    [Tooltip("A multiplier to make the score increase faster. 1 unit of distance = 1 * multiplier score.")]
    public float scoreMultiplier = 1.0f;

    // --- Private Variables ---
    private Vector3 lastPosition;
    private float totalDistance;
    private static float score;

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned in the ScoreManager script!");
            enabled = false; // disable this if can't track player
            return;
        }

        // need to know last position to calculate distance moved
        lastPosition = playerTransform.position;
        UpdateScoreDisplay();
    }

    void Update()
    {
        float distanceThisFrame = Vector3.Distance(playerTransform.position, lastPosition);

        totalDistance += distanceThisFrame;

        lastPosition = playerTransform.position;

        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            int currentScore = (int)(totalDistance * scoreMultiplier);
            score = currentScore;
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }
    public static float getScore() => score;

    public static void trySetHighScore()
    {
        if (score > PlayerPrefs.GetFloat("highScore", 0))
            PlayerPrefs.SetFloat("highScore", score);
    }
}