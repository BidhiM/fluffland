using TMPro;
using UnityEngine;

public class displayScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject[] characterUIObjects;
    void Start(){
        // prolly shouldn't do this here but don't care enough
        string character = PlayerPrefs.GetString("character"); 
        foreach (GameObject obj in characterUIObjects)
        {
            if (obj.name.ToLower() == character.ToLower())
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
        if (scoreText.CompareTag("scoreDisplay"))
        {
            scoreText.text = ScoreManager.getScore().ToString();

            // this is here cuz it should execute only once
            // issue with this being that a score display must be present in every end scene
            // not a pertinent issue so deal with it
            ScoreManager.trySetHighScore();
            BerryManager.setBerries();
        }
        else if (scoreText.CompareTag("highScoreDisplay"))
        {
            // cuz we don't know when the ScoreManager.trySetHighScore() will execute
            // so we just find which score is higher
            // and display the higher one
            if (ScoreManager.getScore() > PlayerPrefs.GetFloat("highScore", 0))
                scoreText.text = ScoreManager.getScore().ToString();
            else
                scoreText.text = PlayerPrefs.GetFloat("highScore", 0).ToString();
        }    
    }
}
