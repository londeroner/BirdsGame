using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDisappear : MonoBehaviour
{
    public List<GameObject> objectsToDisappear;
    public float Alpha = 0.99f;

    private void OnTriggerEnter(Collider other)
    {
        var birdFormation = other.transform.parent?.parent?.parent;

        if (birdFormation is not null)
        {
            birdFormation.GetComponent<BirdsFormation>().Tree = gameObject;

            if (other.tag == Consts.BirdTag && !string.IsNullOrEmpty(birdFormation.name) && birdFormation.name == Consts.PlayerFormation)
            {
                objectsToDisappear.ForEach(x =>
                {
                    var renderer = x.GetComponent<Renderer>();
                    Color oldColor = renderer.material.color;
                    renderer.material.SetColor("_BaseColor", new Color(oldColor.r, oldColor.g, oldColor.b, Alpha));
                });
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
                objectsToDisappear.ForEach(x =>
                {
                    var renderer = x.GetComponent<Renderer>();
                    Color oldColor = renderer.material.color;
                    renderer.material.SetColor("_BaseColor", new Color(oldColor.r, oldColor.g, oldColor.b, 1.0f));
                });
            }
        }
    }
}
