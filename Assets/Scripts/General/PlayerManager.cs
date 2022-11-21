using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public TextMeshProUGUI foodText;

    public GameObject playerFormation;
    private BirdsFormation birds;

    public GameObject featherPrefab;

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

    public void ChangeResourceText()
    {
        foodText.text = $"������� ���: {birds.CollectedFood}" +
            $"\n������� �����: {birds.CollectedCoins}" +
            $"\n������� ������: {birds.CollectedCaps}" +
            $"\n��������� ���: {string.Format("{0:.##}", birds.CollectedWeight)}/{string.Format("{0:#.##}", GameBalance.instance.MaxWeight)}";
    }
}
