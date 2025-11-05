using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private GameObject[] characters;
    [SerializeField] private TMP_Text berriesDisplay;
    [SerializeField] private TMP_Text highscoreDisplay;

    // order doesn't matter for this one

    private string currentCharacter;

    void Start()
    {
        PlayerPrefs.SetInt("berries", 100); //debug )
        currentCharacter = PlayerPrefs.GetString("character");
        if(currentCharacter.Length == 0) currentCharacter = "thyme";

        // null checking and error logging
        if (currentCharacter == null)
        {
            Debug.LogError("No character selected");
            return;
        }
        if (backgrounds.Length == 0)
        {
            Debug.LogError("No backgrounds assigned");
            return;
        }
        if (characters.Length == 0)
        {
            Debug.LogError("No characters assigned");
            return;
        }
        if (characters.Length != backgrounds.Length)
        {
            Debug.LogError("characters and backgrounds length missmatch");
            return;
        }
        if (!berriesDisplay)
        {
            Debug.LogError("berriesDisplay is not attached");
            return;
        }
        if (!highscoreDisplay)
        {
            Debug.LogError("highscoreDisplay is not attached");
            return;
        }


        // the actual meat

        foreach (GameObject background in backgrounds)
        {
            if (background.name.Replace("Menu", "") == currentCharacter)
                background.SetActive(true); // ensure what we want is on.
            else
                background.SetActive(false); // ensure everything else is off
        }

        foreach (GameObject character in characters)
        {
            if (character.name == currentCharacter)
            {
                character.SetActive(true);
                if(character.name == "draco")
                {
                    character.GetComponent<Animator>().SetBool("idling", true);
                }
            }
            else
                character.SetActive(false);
        }

        highscoreDisplay.text = PlayerPrefs.GetFloat("highScore", 0).ToString();
        berriesDisplay.text = PlayerPrefs.GetInt("berries", 0).ToString();
    }

    public void CharacterSelectButtonClick()
    {
        SceneManager.LoadSceneAsync("characterSelect");
    }

    public void StartButtonClick()
    {
        SceneManager.LoadSceneAsync("SampleScene");
        Debug.Log("Started scene transition");
    }

}
