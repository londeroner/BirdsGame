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
    public List<Collectible> collectedResources = new List<Collectible>();
    [NonSerialized]
    public byte maxResourceCount = 3;

    private BirdEffectManager _effectManager;

    [NonSerialized]
    public GameObject Tree;
    void Awake()
    {
        maxHealth = Health;
        chargeActive = ChargeActive();
        abilityActive = AbilityActive();
        _effectManager = GetComponent<BirdEffectManager>();
    }

    public void ChangeFormationTypeFromButton(int type)
    {
        switch (type)
        {
            case 0:
                ChangeFormationType(FormationType.AttackFormation);
                break;
            case 1:
                ChangeFormationType(FormationType.DefenceFormation);
                break;
            case 2:
                ChangeFormationType(FormationType.CollectFormation);
                break;
        }
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

        if (IsPlayer) UIManager.instance.PlayerChangeFormation(type);
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
            GameObject f = Instantiate(PlayerManager.instance.featherPrefab, gameObject.transform.position, new Quaternion());
            f.GetComponentInChildren<Collectible>().Drop();
            f.GetComponentInChildren<Collectible>().Generated = true;
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
                UIManager.instance.ChangeActiveButton(FormationStats.GetActiveCooldown() - FormationStats.GetCooldown());
        }
        if (IsPlayer)
            UIManager.instance.ChangeActiveButton(0);

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
        Debug.Log(collectedResources.Count);
        if (collectedResources.Count == maxResourceCount) return false;
        else collectedResources.Add(collectible);

        switch (collectible.Type)
        {
            case CollectibleResource.Apple:
            case CollectibleResource.Blueberry:
            case CollectibleResource.Cranberry:
                if (FormationStats.FormationType == FormationType.CollectFormation && isAbilityActive)
                {
                    if (Health < maxHealth)
                    {
                        Health += 2;
                        if (Health > maxHealth) Health = maxHealth;
                        healthBar.fillAmount = ((float)Health * 100 / (float)maxHealth) / 100;
                    }
                }
                break;
            case CollectibleResource.Coin:
                break;
            case CollectibleResource.Cap:
                break;
            case CollectibleResource.Feather:
                break;
            default: throw new System.Exception("No resource type");
        }

        if (IsPlayer) UIManager.instance.RedrawInventory();

        return true;
    }
    public void DropFromInventoryResource(int index)
    {
        if (collectedResources.Count > index)
        {
            Collectible drop = collectedResources[index];
            GameObject prefabToDrop = null;
            switch (drop.Type)
            {
                case CollectibleResource.Apple:
                    prefabToDrop = GenerationSystem.instance.applePrefab;
                    break;
                case CollectibleResource.Coin:
                    prefabToDrop = GenerationSystem.instance.coinPrefab;
                    break;
                case CollectibleResource.Cap:
                    prefabToDrop = GenerationSystem.instance.capPrefab;
                    break;
                case CollectibleResource.Feather:
                    prefabToDrop = PlayerManager.instance.featherPrefab;
                    break;
                case CollectibleResource.Blueberry:
                    prefabToDrop = GenerationSystem.instance.blueberryPrefab;
                    break;
                case CollectibleResource.Cranberry:
                    prefabToDrop = GenerationSystem.instance.cranberryPrefab;
                    break;
            }

            GameObject f = Instantiate(prefabToDrop, gameObject.transform.position, new Quaternion());
            f.transform.Rotate(0f, new System.Random().Next(0, 360), 0f);
            f.GetComponentInChildren<Collectible>().Drop();
            f.GetComponentInChildren<Collectible>().Generated = true;

            collectedResources.Remove(drop);
            UIManager.instance.RedrawInventory();
        }
    }

    private Vector3 GetCoordCenterWithYAdd(Vector3 a, Vector3 b, float yadd)
    {
        float x = (a.x + b.x) / 2;
        float y = (a.y + b.y) / 2;
        float z = (a.z + b.z) / 2;

        return new Vector3(x, y + yadd, z);
    }
}
