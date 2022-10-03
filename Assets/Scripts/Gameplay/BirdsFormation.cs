using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsFormation : MonoBehaviour
{
    public int Health = 10;

    [NonSerialized]
    public bool _fight = false;

    private void Update()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Consts.FormationTag && !_fight)
        {
            Health--;
            _fight = true;

            var otherFormation = other.gameObject.GetComponent<BirdsFormation>();
            otherFormation.Health--;
            otherFormation._fight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Consts.FormationTag)
            _fight = false;
    }
}
