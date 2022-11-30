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
    
    public FormationStats FormationStats = new FormationStats();

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
    [NonSerialized]
    public int CollectedFeathers = 0;

    private int _collectedFormationFood = 0;

    public TextMeshProUGUI ActiveButtonText;
    private BirdEffectManager _effectManager;

    public GameObject AttackButton;
    public GameObject DefenceButton;
    public GameObject CollectButton;

    [NonSerialized]
    public GameObject Tree;
    void Awake()
    {
        maxHealth = Health;
        chargeActive = ChargeActive();
        abilityActive = AbilityActive();
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
        _effectManager.DisableAll();

        FormationStats.FormationType = type;

        StartCoroutine(chargeActive);

        Debug.Log($"Current formation type: {type}");
        if (IsPlayer) SetButtonsColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Consts.FormationTag && !_fight)
        {
            var otherForm = other.gameObject.GetComponent<BirdsFormation>();
            
            TakeDamage(otherForm.FormationStats.GetDamageAmount() - FormationStats.GetDamageReduce(isAbilityActive), otherForm);
            otherForm.TakeDamage(FormationStats.GetDamageAmount() - otherForm.FormationStats.GetDamageReduce(otherForm.isAbilityActive), this);
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
            Instantiate(PlayerManager.instance.deathEffectPrefab, gameObject.transform.position, new Quaternion());
            Instantiate(PlayerManager.instance.featherPrefab, gameObject.transform.position, new Quaternion());
            Destroy(gameObject);
        }
        else
        {
            Instantiate(PlayerManager.instance.clashEffectPrefab, GetCoordCenterWithYAdd(gameObject.transform.position, source.transform.position, 1f), new Quaternion());
        }
    }

    private IEnumerator ChargeActive()
    {
        while (FormationStats.GetCooldown() < FormationStats.GetActiveCooldown())
        {
            yield return null;
            FormationStats.AddCooldown(Time.deltaTime);
            if (IsPlayer)
                ActiveButtonText.text = string.Format("{0:#.##}", FormationStats.GetActiveCooldown() - FormationStats.GetCooldown());
        }
        if (IsPlayer)
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
        _effectManager.ChangeEffect(FormationStats.FormationType);
        isAbilityActive = true;
        FormationStats.ResetCooldown();
        canActivateAbility = false;
        yield return new WaitForSeconds(FormationStats.GetActiveDuration());
        isAbilityActive = false;
        _effectManager.DisableAll();
        StartCoroutine(chargeActive);
    }

    public bool CollectResource(Collectible collectible)
    {
        if (CollectedWeight + collectible.Weight > GameBalance.instance.MaxWeight)
            return false;

        switch (collectible.Type)
        {
            case CollectibleResource.Apple:
            case CollectibleResource.Blueberry:
            case CollectibleResource.Cranberry:
                CollectedFood += 1 * FormationStats.GetResourceMultiplier(isAbilityActive);
                CollectedWeight += collectible.Weight * FormationStats.GetResourceMultiplier(isAbilityActive);

                if (FormationStats.FormationType == FormationType.CollectFormation)
                {
                    _collectedFormationFood++;
                    if (_collectedFormationFood >= 2)
                    {
                        if (Health < maxHealth)
                        {
                            Health++;
                            healthBar.fillAmount = ((float)Health * 100 / (float)maxHealth) / 100;
                        }
                        _collectedFormationFood = 0;
                    }
                }
                break;
            case CollectibleResource.Coin:
                CollectedCoins++;
                CollectedWeight += collectible.Weight;
                break;
            case CollectibleResource.Cap:
                CollectedCaps++;
                CollectedWeight += collectible.Weight;
                break;
            case CollectibleResource.Feather:
                CollectedFeathers++;
                CollectedWeight += collectible.Weight;
                break;
            default: throw new System.Exception("No resource type");
        }

        if (IsPlayer) PlayerManager.instance.ChangeResourceText();

        return true;
    }

    private Vector3 GetCoordCenterWithYAdd(Vector3 a, Vector3 b, float yadd)
    {
        float x = (a.x + b.x) / 2;
        float y = (a.y + b.y) / 2;
        float z = (a.z + b.z) / 2;

        return new Vector3(x, y + yadd, z);
    }

    private void SetButtonsColor()
    {
        var attackButtonColor = AttackButton.GetComponent<Image>();
        var defenceButtonColor = DefenceButton.GetComponent<Image>();
        var collectButtonColor = CollectButton.GetComponent<Image>();

        attackButtonColor.color = Color.white;
        defenceButtonColor.color = Color.white;
        collectButtonColor.color = Color.white;

        switch (FormationStats.FormationType)
        {
            case FormationType.AttackFormation:
                attackButtonColor.color = Color.red;
                break;
            case FormationType.DefenceFormation:
                defenceButtonColor.color = Color.red;
                break;
            case FormationType.CollectFormation:
                collectButtonColor.color = Color.red;
                break;
        }
    }
}
