using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GenerationPoint
{
    public GameObject generationPoint;
    public Quaternion rotation;
    public CollectibleResource resourceType;
    public bool useGravity = false;
    [NonSerialized]
    public bool isFilled = false;
}
