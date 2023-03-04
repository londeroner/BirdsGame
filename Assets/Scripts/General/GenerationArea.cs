using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationArea : MonoBehaviour
{
    public List<GenerationPoint> points;

    void Awake()
    {
        GenerationSystem.instance.areas.Add(this);
    }

    public bool GenerateResource(int iteration, CollectibleResource targetType)
    {
        foreach (var point in points)
        {
            if (point.resourceType == targetType && !point.isFilled)
            {
                if (new System.Random().Next(0, 11) < (iteration + 1) * 2)
                {
                    GameObject generated = null;
                    switch (point.resourceType)
                    {
                        case CollectibleResource.Apple:
                            generated = Instantiate(GenerationSystem.instance.applePrefab, point.generationPoint.transform.position, point.rotation);
                            break;
                        case CollectibleResource.Coin:
                            generated = Instantiate(GenerationSystem.instance.coinPrefab, point.generationPoint.transform.position, point.rotation);
                            break;
                        case CollectibleResource.Cap:
                            generated = Instantiate(GenerationSystem.instance.capPrefab, point.generationPoint.transform.position, point.rotation);
                            break;
                        case CollectibleResource.Blueberry:
                            generated = Instantiate(GenerationSystem.instance.blueberryPrefab, point.generationPoint.transform.position, point.rotation);
                            break;
                        case CollectibleResource.Cranberry:
                            generated = Instantiate(GenerationSystem.instance.cranberryPrefab, point.generationPoint.transform.position, point.rotation);
                            break;
                    }
                    if (!point.useGravity)
                    {

                    }
                    generated.GetComponentInChildren<Rigidbody>().useGravity = point.useGravity;
                    point.isFilled = true;
                    return true;
                }
            }
        }
        return false;
    }
}
