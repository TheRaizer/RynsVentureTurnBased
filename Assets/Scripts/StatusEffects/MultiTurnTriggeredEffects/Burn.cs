using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : StatusEffect
{
    private const float PERCENT_DEMINISH = 0.12f;

    public override void OnTurn(List<StatsManager> attackableTeam, List<StatsManager> opposingTeam, StatsManager infectee, BattleLogic battleLogic)
    {
        base.OnTurn(attackableTeam, opposingTeam, infectee, battleLogic);

        int amtToHit = MathExtension.RoundToNearestInteger(infectee.HealthManager.MaxHealth * PERCENT_DEMINISH);
        if (amtToHit == 0) amtToHit = 1;

        infectee.HealthManager.Hit(amtToHit);
    }

    public override StatusEffect ShallowCopy()
    {
        return (Burn)MemberwiseClone();
    }
}
