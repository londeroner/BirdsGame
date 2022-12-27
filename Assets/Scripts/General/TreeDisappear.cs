using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeDisappear : MonoBehaviour
{
    public List<GameObject> objectsToDisappear;
    public float Alpha = 0.99f;

    public bool isHomeTree = false;

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
                    renderer.material.SetColor("_Color", new Color(oldColor.r, oldColor.g, oldColor.b, Alpha));
                });
            }

            if (isHomeTree && birdFormation.GetComponent<BirdsFormation>().IsPlayer)
            {
                UIManager.instance.ChangeHomeButton(true);
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
                    renderer.material.SetColor("_Color", new Color(oldColor.r, oldColor.g, oldColor.b, 1.0f));
                });
            }

            if (isHomeTree)
            {
                UIManager.instance.ChangeHomeButton(true);
            }
        }

    }
}
