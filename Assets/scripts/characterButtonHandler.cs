using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterButtonHandler : MonoBehaviour
{
    [SerializeField] private CharacterSelectScript selectionController;
    [SerializeField] private TMP_Text characterNameDisplayField;
    [SerializeField] private TMP_Text berryDisplay;
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
        //set the berries
    }

    public void OnButtonClick()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        if (clickedObject == null)
        {
            Debug.LogError("Clicked OBJ is null");
            return;
        }

        if (clickedObject.name== "rightButton") selectionController.NavigateRight();
        else if (clickedObject.name == "leftButton") selectionController.NavigateLeft();
    }

    public void changeDisplayedName(string name)
    {
        if (!characterNameDisplayField) return;
        characterNameDisplayField.text = name;
    }

}
