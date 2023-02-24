using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject playerFormation;
    public BirdsFormation birds;

    public GameObject featherPrefab;
    public GameObject deathEffectPrefab;
    public GameObject clashEffectPrefab;

    public int LevelCollectedFood = 0;
    public int LevelCollectedCoins = 0;
    public int LevelCollectedCaps = 0;
    public int LevelCollectedFeathers = 0;

    public PlayerManager()
    {
        if (instance == null)
            instance = this;
        else throw new System.Exception("Player manager allready created");
    }

    private void Start()
    {
        birds = playerFormation.GetComponent<BirdsFormation>();
        StartCoroutine(EndLevel());
    }

    public void ReturnHome()
    {
        foreach (var collectible in birds.collectedResources)
        {
            switch (collectible.Type)
            {
                case CollectibleResource.Apple:
                case CollectibleResource.Blueberry:
                case CollectibleResource.Cranberry:
                    LevelCollectedFood++;
                    break;
                case CollectibleResource.Coin:
                    LevelCollectedCoins++;
                    break;
                case CollectibleResource.Cap:
                    LevelCollectedCaps++;
                    break;
                case CollectibleResource.Feather:
                    LevelCollectedFeathers++;
                    break;
            }
        }
        birds.collectedResources.Clear();
        UIManager.instance.UpdateCollectedResources();
        UIManager.instance.RedrawInventory();
    }

    private IEnumerator EndLevel()
    {
        float time = GameBalance.instance.LevelTimer;
        int i = 0;
        UIManager.instance.UpdateLevelTimer(time - i);
        while (i < time)
        {
            yield return new WaitForSeconds(1f);
            i++;
            UIManager.instance.UpdateLevelTimer(time - i);
        }

        SaveResourceProgress();
        SceneManager.LoadScene(0);
    }

    private void SaveResourceProgress()
    {
        PlayerPrefs.SetInt(ConstNames.FoodPrefs, LevelCollectedFood + PlayerPrefs.GetInt(ConstNames.FoodPrefs, 0));
        PlayerPrefs.SetInt(ConstNames.CoinPrefs, LevelCollectedCoins + PlayerPrefs.GetInt(ConstNames.CoinPrefs, 0));
        PlayerPrefs.SetInt(ConstNames.CapsPrefs, LevelCollectedCaps + PlayerPrefs.GetInt(ConstNames.CapsPrefs, 0));
        PlayerPrefs.SetInt(ConstNames.FeatherPrefs, LevelCollectedFeathers + PlayerPrefs.GetInt(ConstNames.FeatherPrefs, 0));
    }
}
