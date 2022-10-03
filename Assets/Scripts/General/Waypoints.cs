using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] WayPoints { get; set; }

    void Awake()
    {
        WayPoints = new Transform[transform.childCount];
        for (int i = 0; i < WayPoints.Length; i++)
            WayPoints[i] = transform.GetChild(i);
    }

    void Update()
    {

    }
}
