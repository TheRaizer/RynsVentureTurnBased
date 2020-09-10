﻿using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    private const float PERCENT_DEMINISH = 0.07f;

    public override void OnTurn(List<StatsManager> attackableTeam, List<StatsManager> opposingTeam, StatsManager infectee, StateMachine battleStateMachine, TextBoxHandler textBoxHandler)
    {
        base.OnTurn(attackableTeam, opposingTeam, infectee, battleStateMachine, textBoxHandler);

        int amtToHit = MathExtension.RoundToNearestInteger(infectee.HealthManager.MaxHealth * PERCENT_DEMINISH);
        if (amtToHit == 0) amtToHit = 1;

        infectee.HealthManager.Hit(amtToHit);
    }

    public override StatusEffect ShallowCopy()
    {
        return (Poison)MemberwiseClone();
    }
}