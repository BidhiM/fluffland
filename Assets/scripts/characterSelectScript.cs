using UnityEngine;

public class CharacterSelectScript : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;

    private int currentIndex = 0;

    void Start()
    {
        for (int i = 0; i < characters.Length; i++)
            characters[i].SetActive(i == currentIndex);
        //keep all off right now, except 0th place. that should be thyme
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % characters.Length;
            characters[currentIndex].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            characters[currentIndex].SetActive(false);
            currentIndex = (currentIndex - 1 + characters.Length) % characters.Length;
            characters[currentIndex].SetActive(true);
        }
    }

}
