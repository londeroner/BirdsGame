using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private int _collectedFood = 0;
    public int CollectedFood
    {
        get => _collectedFood;
        set
        {
            _collectedFood = value;
            foodText.text = $"Собрано еды: {CollectedFood}";
        }
    }

    public TextMeshProUGUI foodText;

    public PlayerManager()
    {
        if (instance == null)
            instance = this;
        else throw new System.Exception("Player manager allready created");
    }
}
