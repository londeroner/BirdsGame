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

    [NonSerialized]
    public FormationStats FormationStats;

    [NonSerialized]
    public bool isAbilityActive = false;

    private IEnumerator chargeActive;

    void Start()
    {
        maxHealth = Health;
        chargeActive = ChargeActive();
        FormationStats = new FormationStats();
    }

    public void ChangeFormationType(FormationType type)
    {
        StopCoroutine(chargeActive);
        chargeActive = ChargeActive();

        FormationStats.FormationType = type;

        StartCoroutine(chargeActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Consts.FormationTag && !_fight)
        {
            var otherForm = other.gameObject.GetComponent<BirdsFormation>();
            
            TakeDamage(otherForm.FormationStats.GetDamageAmount() - FormationStats.GetDamageReduce(isAbilityActive), otherForm);
            otherForm.TakeDamage(FormationStats.GetDamageAmount() - otherForm.FormationStats.GetDamageReduce(isAbilityActive), this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Consts.FormationTag)
            _fight = false;
    }

    private void TakeDamage(int amount, BirdsFormation source)
    {
        Health -= amount;
        _fight = true;
        healthBar.fillAmount = ((float)Health * 100 / (float)maxHealth) / 100;

        if (Health <= 0)
        {
            if (source.FormationStats.FormationType == FormationType.AttackFormation)
                source.Health++;
            Destroy(gameObject);
        }
    }

    private IEnumerator ChargeActive()
    {
        var cooldown = FormationStats.GetActiveCooldown();

        Debug.Log($"Current cooldown: {cooldown}");

        yield return new WaitForSeconds(cooldown);

        Debug.Log("Active charged");
    }
}
