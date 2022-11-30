using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI CollectedResources;

    public void Start()
    {
        LoadCollectedResources();
    }

    public void LoadLevel(int levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void LoadCollectedResources()
    {
        CollectedResources.text = $"Собрано еды: {PlayerPrefs.GetInt(ConstNames.FoodPrefs, 0)}" +
            $"\nСобрано монет: {PlayerPrefs.GetInt(ConstNames.CoinPrefs, 0)}" +
            $"\nСобрано крышек: {PlayerPrefs.GetInt(ConstNames.CapsPrefs, 0)}" +
            $"\nСобрано перьев: {PlayerPrefs.GetInt(ConstNames.FeatherPrefs, 0)}";
    }
}
