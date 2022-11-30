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
        CollectedResources.text = $"������� ���: {PlayerPrefs.GetInt(ConstNames.FoodPrefs, 0)}" +
            $"\n������� �����: {PlayerPrefs.GetInt(ConstNames.CoinPrefs, 0)}" +
            $"\n������� ������: {PlayerPrefs.GetInt(ConstNames.CapsPrefs, 0)}" +
            $"\n������� ������: {PlayerPrefs.GetInt(ConstNames.FeatherPrefs, 0)}";
    }
}
