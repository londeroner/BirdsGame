using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FormationStats
{
    [NonSerialized]
    public float AttackRecharged = 0f;
    [NonSerialized]
    public float DefenceRecharged = 0f;
    [NonSerialized]
    public float CollectRecharged = 0f;

    public int BaseDamage = 1;
    public int AttackFormationDamage = 2;

    [Header("Formations speed")]
    public float AttackSpeed = 6f;
    public float AttackActiveSpeed = 8f;
    public float OtherSpeed = 4.5f;

    [Header("Formations turn speed")]
    public float AttackTurn = 1.5f;
    public float DefenceTurn = 4.5f;
    public float CollectTurn = 3f;

    [Header("Active cooldowns")]
    public float AttackCooldown = 10f;
    public float DefenceCooldown = 15f;
    public float CollectCooldown = 15f;

    [Header("Active durations")]
    public float AttackDuration = 5f;
    public float DefenceDuration = 5f;
    public float CollectDuration = 3f;

    public FormationType FormationType { get; set; }

    public int GetDamageAmount() => FormationType switch
    {
        FormationType.AttackFormation => AttackFormationDamage,
        _ => BaseDamage
    };

    public float GetMaxSpeed(bool isActive) => FormationType switch
    {
        FormationType.AttackFormation => isActive ? AttackActiveSpeed : AttackSpeed,
        FormationType.DefenceFormation => OtherSpeed,
        FormationType.CollectFormation => OtherSpeed,
        _ => 3
    };

    public float GetTurnSpeed() => FormationType switch
    {
        FormationType.NeutralFormation => 100000f,
        FormationType.AttackFormation => AttackTurn,
        FormationType.DefenceFormation => DefenceTurn,
        FormationType.CollectFormation => CollectTurn,
        _ => 0f
    };

    public float GetActiveCooldown() => FormationType switch
    {
        FormationType.AttackFormation => AttackCooldown,
        FormationType.DefenceFormation => DefenceCooldown,
        FormationType.CollectFormation => CollectCooldown,
        _ => 0,
    };

    public float GetActiveDuration() => FormationType switch
    {
        FormationType.AttackFormation => AttackDuration,
        FormationType.DefenceFormation => DefenceDuration,
        FormationType.CollectFormation => CollectDuration,
        _ => 0f
    };

    public int GetResourceMultiplier(bool isActive) => FormationType switch
    {
        FormationType.CollectFormation => isActive ? 2 : 1,
        _ => 1
    };

    public int GetDamageReduce(bool isActive) => FormationType switch
    {
        FormationType.DefenceFormation => isActive ? 1 : 0,
        _ => 0
    };

    public float GetCooldown() => FormationType switch
    {
        FormationType.AttackFormation => AttackRecharged,
        FormationType.DefenceFormation => DefenceRecharged,
        FormationType.CollectFormation => CollectRecharged,
        _ => 0f
    };

    public void AddCooldown(float cooldown)
    {
        switch (FormationType)
        {
            case FormationType.AttackFormation:
                AttackRecharged += cooldown;
                break;
            case FormationType.DefenceFormation:
                DefenceRecharged += cooldown;
                break;
            case FormationType.CollectFormation:
                CollectRecharged += cooldown;
                break;
        }
    }

    public void ResetCooldown()
    {
        switch (FormationType)
        {
            case FormationType.AttackFormation:
                AttackRecharged = 0f;
                break;
            case FormationType.DefenceFormation:
                DefenceRecharged = 0f;
                break;
            case FormationType.CollectFormation:
                CollectRecharged = 0f;
                break;
        }
    }
}
