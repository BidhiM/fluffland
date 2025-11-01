using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterButtonHandler : MonoBehaviour
{
    [SerializeField] private CharacterSelectScript selectionController;
    [SerializeField] private TMP_Text characterNameDisplayField;
    [SerializeField] private TMP_Text berryDisplay;
    private bool selected = false;
    void Start()
    {
        if (!berryDisplay)
        {
            Debug.LogError("Berry display is not attached");
            return;
        }
        if (!characterNameDisplayField)
        {
            Debug.LogError("characterNameDisplayField is not attached");
            return;
        }
        if (!selectionController)
        {
            Debug.LogError("selectionController is not attached");
            return;
        }

        berryDisplay.text = PlayerPrefs.GetInt("berries", 0).ToString();
        ChangeDisplayedName(selectionController.GetCurrentCharacterName()); // set it initially so its not blank
        //set the berries
    }
    public void rightButtonClick()
    {
        selectionController.NavigateRight();
    }

    public void leftButtonClick()
    {
        selectionController.NavigateLeft();
    }

    public void characterSelectButtonClick()
    {
        if (!selected)
        {
            PlayerPrefs.SetString("character", selectionController.GetCurrentCharacterName());
            Debug.Log("Current Character");
            SceneManager.LoadScene("SampleScene");
            selected = true; // can't select more than once
        }
    }

    public void ChangeDisplayedName(string name)
    {
        characterNameDisplayField.text = name;
    }

}
