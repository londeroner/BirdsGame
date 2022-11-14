using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationSystem : MonoBehaviour
{
    public List<GenerationArea> areas;

    [Header("Resources Prefabs")]
    public GameObject applePrefab;
    public GameObject cranberryPrefab;
    public GameObject blueberryPrefab;
    public GameObject coinPrefab;
    public GameObject capPrefab;

    [Header("Target count")]
    public int appleCount = 5;
    public int cranberryCount = 5;
    public int blueberryCount = 5;
    public int coinCount = 5;
    public int capCount = 5;

    private int appleCountGenerated = 0;
    private int cranberryCountGenerated = 0;
    private int blueberryCountGenerated = 0;
    private int coinCountGenerated = 0;
    private int capCountGenerated = 0;

    private bool appleGenerated = false;
    private bool cranberryGenerated = false;
    private bool blueberryGenerated = false;
    private bool coinGenerated = false;
    private bool capGenerated = false;

    [NonSerialized]
    public static GenerationSystem instance;
    public GenerationSystem()
    {
        if (instance == null)
            instance = this;
        else throw new System.Exception("More than 1 GenerationSystem instance");
    }

    void Start()
    {
        int iteration = 0;
        GenerateResources(iteration);
    }

    void GenerateResources(int iteration)
    {
        if (!appleGenerated && appleCountGenerated < appleCount)
        {
            GenerateResourceType(iteration, CollectibleResource.Apple);
        }
        if (!cranberryGenerated && cranberryCountGenerated < cranberryCount)
        {
            GenerateResourceType(iteration, CollectibleResource.Cranberry);
        }
        if (!blueberryGenerated && blueberryCountGenerated < blueberryCount)
        {
            GenerateResourceType(iteration, CollectibleResource.Blueberry);
        }
        if (!coinGenerated && coinCountGenerated < coinCount)
        {
            GenerateResourceType(iteration, CollectibleResource.Coin);
        }
        if (!capGenerated && capCountGenerated < capCount)
        {
            GenerateResourceType(iteration, CollectibleResource.Cap);
        }

        if (!(appleGenerated && cranberryGenerated && blueberryGenerated && coinGenerated && capGenerated) && iteration < 6)
            GenerateResources(++iteration);
    }

    void GenerateResourceType(int iteration, CollectibleResource type)
    {
        foreach (var area in areas)
        {
            if (area.GenerateResource(iteration, type))
            {
                switch (type)
                {
                    case CollectibleResource.Apple:
                        appleCountGenerated++;
                        if (appleCountGenerated == appleCount)
                        {
                            appleGenerated = true;
                            return;
                        }
                        break;
                    case CollectibleResource.Coin:
                        coinCountGenerated++;
                        if (coinCountGenerated == coinCount)
                        {
                            coinGenerated = true;
                            return;
                        }
                        break;
                    case CollectibleResource.Cap:
                        capCountGenerated++;
                        if (capCountGenerated == capCount)
                        {
                            capGenerated = true;
                            return;
                        }
                        break;
                    case CollectibleResource.Blueberry:
                        blueberryCountGenerated++;
                        if (blueberryCountGenerated == blueberryCount)
                        {
                            blueberryGenerated = true;
                            return;
                        }
                        break;
                    case CollectibleResource.Cranberry:
                        cranberryCountGenerated++;
                        if (cranberryCountGenerated == cranberryCount)
                        {
                            cranberryGenerated = true;
                            return;
                        }
                        break;
                }
            }
        }
    }
}
