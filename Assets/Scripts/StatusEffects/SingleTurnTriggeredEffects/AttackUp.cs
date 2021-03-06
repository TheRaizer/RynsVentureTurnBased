﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : StatusEffect
{
    [SerializeField] private float damageScaleIncrease = 1.5f;
    public override void OnEffectStart(StatsManager inhabitor)
    {
        base.OnEffectStart(inhabitor);
        inhabitor.DamageScale += damageScaleIncrease;
    }

    public override void OnWornOff(StatsManager inhabitor)
    {
        base.OnWornOff(inhabitor);

        inhabitor.DamageScale -= damageScaleIncrease;
    }

    public override StatusEffect ShallowCopy()
    {
        return (AttackUp)MemberwiseClone();
    }
}
