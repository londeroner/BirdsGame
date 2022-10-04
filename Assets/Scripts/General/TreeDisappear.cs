using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDisappear : MonoBehaviour
{
    public GameObject objectToDisappear;

    private void OnTriggerEnter(Collider other)
    {
        var birdFormation = other.transform.parent?.parent?.parent?.name;
        if (other.tag == Consts.BirdTag && !string.IsNullOrEmpty(birdFormation) && birdFormation == Consts.PlayerFormation)
        {
            objectToDisappear.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var birdFormation = other.transform.parent?.parent?.parent?.name;
        if (other.tag == Consts.BirdTag && !string.IsNullOrEmpty(birdFormation) && birdFormation == Consts.PlayerFormation)
        {
            objectToDisappear.SetActive(true);
        }
    }
}
