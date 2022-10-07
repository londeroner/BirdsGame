using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationStats
{
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
        FormationType.AttackFormation => 10,
        FormationType.DefenceFormation => 15,
        FormationType.CollectFormation => 15,
        _ => 0,
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
}
