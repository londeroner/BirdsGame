using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BirdsFormation : MonoBehaviour
{
    public int Health = 10;
    private int maxHealth;

    public bool IsPlayer = false;

    [NonSerialized]
    public bool _fight = false;

    [Header("Unity Stuff")]
    public Image healthBar;

    [NonSerialized]
    public FormationStats FormationStats;

    [NonSerialized]
    public bool isAbilityActive = false;
    [NonSerialized]
    public bool canActivateAbility = false;

    private IEnumerator chargeActive;
    private IEnumerator abilityActive;

    [NonSerialized]
    public float CollectedWeight = 0;
    [NonSerialized]
    public int CollectedFood = 0;
    [NonSerialized]
    public int CollectedCoins = 0;
    [NonSerialized]
    public int CollectedCaps = 0;

    private int _collectedFormationFood = 0;

    public TextMeshProUGUI ActiveButtonText;
    private BirdEffectManager _effectManager;

    void Start()
    {
        maxHealth = Health;
        chargeActive = ChargeActive();
        abilityActive = AbilityActive();
        FormationStats = new FormationStats();
        _effectManager = GetComponent<BirdEffectManager>();
    }

    public void ChangeFormationType(FormationType type)
    {
        if (FormationStats.FormationType == type) return;

        StopCoroutine(chargeActive);
        chargeActive = ChargeActive();
        StopCoroutine(abilityActive);
        abilityActive = AbilityActive();
        isAbilityActive = false;
        canActivateAbility = false;

        FormationStats.FormationType = type;

        StartCoroutine(chargeActive);

        _effectManager.ChangeEffect(type);
        Debug.Log($"Current formation type: {type}");
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

        if (Health <= 0)
        {
            if (source.FormationStats.FormationType == FormationType.AttackFormation)
                source.Health++;
            Destroy(gameObject);
        }
        healthBar.fillAmount = ((float)Health * 100 / (float)maxHealth) / 100;
    }

    private IEnumerator ChargeActive()
    {
        while (FormationStats.GetCooldown() < FormationStats.GetActiveCooldown())
        {
            yield return null;
            FormationStats.AddCooldown(Time.deltaTime);
            ActiveButtonText.text = string.Format("{0:#.##}", FormationStats.GetActiveCooldown() - FormationStats.GetCooldown());
        }
        ActiveButtonText.text = "Active to use";

        canActivateAbility = true;
    }

    public void ActivateAbility()
    {
        if (canActivateAbility)
        {
            StopCoroutine(chargeActive);
            chargeActive = ChargeActive();

            StopCoroutine(abilityActive);
            abilityActive = AbilityActive();
            StartCoroutine(abilityActive);
        }
    }

    public IEnumerator AbilityActive()
    {
        isAbilityActive = true;
        FormationStats.ResetCooldown();
        yield return new WaitForSeconds(FormationStats.GetActiveDuration());
        isAbilityActive = false;
        StartCoroutine(chargeActive);
    }

    public bool CollectResource(Collectible collectible)
    {
        if (CollectedWeight + collectible.Weight > GameBalance.instance.MaxWeight)
            return false;

        switch (collectible.Type)
        {
            case CollectibleResource.Food:
                CollectedFood += 1 * FormationStats.GetResourceMultiplier(isAbilityActive);

                if (FormationStats.FormationType == FormationType.CollectFormation)
                {
                    _collectedFormationFood++;
                    if (_collectedFormationFood >= 2)
                    {
                        Health++;
                        _collectedFormationFood = 0;
                    }
                }
                break;
            case CollectibleResource.Coin:
                CollectedCoins++;
                break;
            case CollectibleResource.Cap:
                CollectedCaps++;
                break;
            default: throw new System.Exception("No resource type");
        }
        CollectedWeight += collectible.Weight * FormationStats.GetResourceMultiplier(isAbilityActive);

        if (IsPlayer) PlayerManager.instance.ChangeResourceText();

        return true;
    }
}
