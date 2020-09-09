using System.Collections.Generic;

public class PoisonEffect : StatusEffect
{
    private const float PERCENT_DEMINISH = 0.07f;

    public override void OnTurn(List<StatsManager> attackableTeam, List<StatsManager> opposingTeam, StatsManager currentUser, BattleLogic battleLogic)
    {
        base.OnTurn(attackableTeam, opposingTeam, currentUser, battleLogic);

        int amtToHit = MathExtension.RoundToNearestInteger(currentUser.HealthManager.MaxHealth * PERCENT_DEMINISH);
        if (amtToHit == 0) amtToHit = 1;

        currentUser.HealthManager.Hit(amtToHit);
    }
}
