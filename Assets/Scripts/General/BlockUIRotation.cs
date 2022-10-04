using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUIRotation : MonoBehaviour 
{
    public List<Transform> preventRot;

    Quaternion startQuat;

    void Awake()
    {
        startQuat = transform.rotation;
    }

    void Update()
    {
        foreach (var tr in preventRot)
        {
            tr.transform.rotation = Quaternion.Euler(-90, startQuat.y, transform.rotation.z);
        }
    }
}
