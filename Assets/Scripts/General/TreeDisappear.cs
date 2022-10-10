using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDisappear : MonoBehaviour
{
    public List<GameObject> objectsToDisappear;

    private void OnTriggerEnter(Collider other)
    {
        var birdFormation = other.transform.parent?.parent?.parent;

        if (birdFormation is not null)
        {
            birdFormation.GetComponent<BirdsFormation>().Tree = gameObject;

            if (other.tag == Consts.BirdTag && !string.IsNullOrEmpty(birdFormation.name) && birdFormation.name == Consts.PlayerFormation)
            {
                objectsToDisappear.ForEach(x => x.SetActive(false));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var birdFormation = other.transform.parent?.parent?.parent;

        if (birdFormation is not null)
        {
            birdFormation.GetComponent<BirdsFormation>().Tree = null;

            if (other.tag == Consts.BirdTag && !string.IsNullOrEmpty(birdFormation.name) && birdFormation.name == Consts.PlayerFormation)
            {
                objectsToDisappear.ForEach(x => x.SetActive(true));
            }
        }
    }
}
