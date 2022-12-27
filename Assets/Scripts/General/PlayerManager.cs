using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public TextMeshProUGUI foodText;

    public GameObject playerFormation;
    private BirdsFormation birds;

    public GameObject featherPrefab;
    public GameObject deathEffectPrefab;
    public GameObject clashEffectPrefab;

    public PlayerManager()
    {
        if (instance == null)
            instance = this;
        else throw new System.Exception("Player manager allready created");
    }

    private void Start()
    {
        birds = playerFormation.GetComponent<BirdsFormation>();
    }

    public void ReturnHome()
    {
        SaveResourceProgress();
        SceneManager.LoadScene(0);
    }

    private void SaveResourceProgress()
    {
        //PlayerPrefs.SetInt(ConstNames.FoodPrefs, birds.CollectedFood + PlayerPrefs.GetInt(ConstNames.FoodPrefs, 0));
        //PlayerPrefs.SetInt(ConstNames.CoinPrefs, birds.CollectedCoins + PlayerPrefs.GetInt(ConstNames.CoinPrefs, 0));
        //PlayerPrefs.SetInt(ConstNames.CapsPrefs, birds.CollectedCaps + PlayerPrefs.GetInt(ConstNames.CapsPrefs, 0));
        //PlayerPrefs.SetInt(ConstNames.FeatherPrefs, birds.CollectedFeathers + PlayerPrefs.GetInt(ConstNames.FeatherPrefs, 0));
    }
}
