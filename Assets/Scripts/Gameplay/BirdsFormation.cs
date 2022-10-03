using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdsFormation : MonoBehaviour
{
    public int Health = 10;
    private int maxHealth;

    [NonSerialized]
    public bool _fight = false;

    [Header("Unity Stuff")]
    public Image healthBar;

    void Start()
    {
        maxHealth = Health;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Consts.FormationTag && !_fight)
        {
            TakeDamage(1);

            other.gameObject.GetComponent<BirdsFormation>().TakeDamage(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Consts.FormationTag)
            _fight = false;
    }

    private void TakeDamage(int amount)
    {
        Health -= amount;
        _fight = true;
        healthBar.fillAmount = ((float)Health * 100 / (float)maxHealth) / 100;

        if (Health <= 0)
            Destroy(gameObject);
    }
}
