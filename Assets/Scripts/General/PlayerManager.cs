using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [NonSerialized]
    public float CollectedWeight = 0;
    [NonSerialized]
    public int CollectedFood = 0;
    [NonSerialized]
    public int CollectedCoins = 0;
    [NonSerialized]
    public int CollectedCaps = 0;

    public TextMeshProUGUI foodText;

    public PlayerManager()
    {
        if (instance == null)
            instance = this;
        else throw new System.Exception("Player manager allready created");
    }

    public bool CollectResource(Collectible collectible)
    {
        if (CollectedWeight + collectible.Weight > GameBalance.instance.MaxWeight)
            return false;

        switch (collectible.Type)
        {
            case CollectibleResource.Food:
                CollectedFood++;
                break;
            case CollectibleResource.Coin:
                CollectedCoins++;
                break;
            case CollectibleResource.Cap:
                CollectedCaps++;
                break;
            default: throw new System.Exception("No resource type");
        }
        CollectedWeight += collectible.Weight;

        foodText.text = $"Собрано еды: {CollectedFood}" +
            $"\nСобрано монет: {CollectedCoins}" +
            $"\nСобрано крышек: {CollectedCaps}" +
            $"\nСобранный вес: {string.Format("{0:.##}", CollectedWeight)}/{string.Format("{0:#.##}", GameBalance.instance.MaxWeight)}";

        return true;
    }
}
