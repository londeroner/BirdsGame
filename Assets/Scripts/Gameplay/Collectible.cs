using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [NonSerialized]
    public float Weight = 1;

    public CollectibleResource Type;

    public void Awake()
    {
        Weight = GetWeightByType();
    }

    public Collectible(CollectibleResource Type)
    {
        this.Type = Type;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Consts.BirdTag)
        {
            other.transform.parent?.parent?.parent.GetComponent<BirdsFormation>().CollectResource(this);
            Destroy(gameObject);
        }
    }

    private float GetWeightByType() => Type switch
    {
        CollectibleResource.Food => GameBalance.instance.foodWeight,
        CollectibleResource.Coin => GameBalance.instance.coinWeight,
        CollectibleResource.Cap => GameBalance.instance.capWeight,
        _ => 1
    };
}
