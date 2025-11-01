using UnityEngine;

public class characterButtonHandler : MonoBehaviour
{
    private string gameObjectName;
    [SerializeField] private CharacterSelectScript selectionController;
    void Start()
    {
        gameObjectName = this.gameObject.name;
    }

    public void OnButtonClick()
    {
        if (gameObjectName == "rightButton") selectionController.NavigateRight();
        else if (gameObjectName == "leftButton") selectionController.NavigateLeft();
    }

}
