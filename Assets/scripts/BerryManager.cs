using UnityEngine;

public class BerryManager : MonoBehaviour
{
    public static BerryManager Instance;

    public int berryCount = 0;
    public static int staticBerryCount => Instance.berryCount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBerry()
    {
        berryCount++;
        Debug.Log("Berries collected: " + berryCount);
    }
    public static void setBerries()
    {
        int currentBerries = PlayerPrefs.GetInt("berries", 0) + staticBerryCount;
        Debug.Log("Current berries " + PlayerPrefs.GetInt("berries", 0));
        PlayerPrefs.SetInt("berries", currentBerries);
        Debug.Log("Final berries " + PlayerPrefs.GetInt("berries", 0));
    }
}
