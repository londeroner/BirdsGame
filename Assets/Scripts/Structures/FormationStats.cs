using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationStats
{
    public float AttackRecharged = 0f;
    public float DefenceRecharged = 0f;
    public float CollectRecharged = 0f;

    public FormationType FormationType { get; set; }

    public int GetDamageAmount() => FormationType switch
    {
        FormationType.AttackFormation => 2,
        _ => 1
    };

    public float GetMaxSpeed(bool isActive) => FormationType switch
    {
        FormationType.AttackFormation => isActive ? 8 : 6,
        FormationType.DefenceFormation => 4.5f,
        FormationType.CollectFormation => 4.5f,
        _ => 3
    };

    public float GetTurnSpeed() => FormationType switch
    {
        FormationType.NeutralFormation => 100000f,
        FormationType.AttackFormation => 1.5f,
        FormationType.DefenceFormation => 4.5f,
        FormationType.CollectFormation => 3f,
        _ => 0f
    };

    public float GetActiveCooldown() => FormationType switch
    {
        FormationType.AttackFormation => 10f,
        FormationType.DefenceFormation => 15f,
        FormationType.CollectFormation => 15f,
        _ => 0,
    };

    public float GetActiveDuration() => FormationType switch
    {
        FormationType.AttackFormation => 5f,
        FormationType.DefenceFormation => 5f,
        FormationType.CollectFormation => 3f,
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
