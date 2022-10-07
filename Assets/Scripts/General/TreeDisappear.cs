using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDisappear : MonoBehaviour
{
    public List<GameObject> objectsToDisappear;

    private void OnTriggerEnter(Collider other)
    {
        var birdFormation = other.transform.parent?.parent?.parent?.name;
        if (other.tag == Consts.BirdTag && !string.IsNullOrEmpty(birdFormation) && birdFormation == Consts.PlayerFormation)
        {
            objectsToDisappear.ForEach(x => x.SetActive(false));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var birdFormation = other.transform.parent?.parent?.parent?.name;
        if (other.tag == Consts.BirdTag && !string.IsNullOrEmpty(birdFormation) && birdFormation == Consts.PlayerFormation)
        {
            objectsToDisappear.ForEach(x => x.SetActive(true));
        }
    }
}
