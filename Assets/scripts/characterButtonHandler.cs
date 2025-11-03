using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

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

        ChangeDisplayedName(selectionController.GetCurrentCharacterName()); // set it initially so its not blank
        SetBerries();
        //set the berries
    }
    public void RightButtonClick()
    {
        selectionController.NavigateLeft();
    }

    public void LeftButtonClick()
    {
        // don't question it man
        selectionController.NavigateRight();
    }

    public void CharacterSelectButtonClick()
    {
        // only work if not selected already, and character is owned
        if (!selected && selectionController.IsCurrentCharacterOwned())
        {
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene(){
        PlayerPrefs.SetString("character", selectionController.GetCurrentCharacterName());
        Debug.Log("Current Character " + PlayerPrefs.GetString("character"));
        yield return selectionController.SaveOwnedCharacters();
        SceneManager.LoadSceneAsync("MAINMENUNEW");
        selected = true; // can't select more than once
    }

    public void ChangeDisplayedName(string name)
    {
        characterNameDisplayField.text = name;
    }

    public void BuyButtonClick()
    {
        if(selectionController.TryBuyCharacter()){
            Debug.Log("Bought");
            Debug.Log(PlayerPrefs.GetString("ownedCharacters"));
            SetBerries();
        } else Debug.Log("Not enough berries");
    }

    private void SetBerries(){
        berryDisplay.text = PlayerPrefs.GetInt("berries", 0).ToString();
    }

}
