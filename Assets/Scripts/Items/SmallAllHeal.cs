using System.Collections.Generic;
using UnityEngine;

public class SmallAllHeal : Useable
{
    [SerializeField] private int amtToHeal = 4;

    public override void OnUseInRoam(StatsManager statsManager, List<StatsManager> friendlyStats)
    {
        base.OnUseInRoam(statsManager, friendlyStats);

        foreach(StatsManager s in friendlyStats)
        {
            s.HealthManager.RegenAmount(amtToHeal);
        }
    }
    public override void OnUseInBattle(StatsManager StatsToHeal, List<StatsManager> friendlyStats, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        foreach (StatsManager s in friendlyStats)
        {
            if (s.HealthManager.Dead) continue;
            s.HealthManager.RegenAmount(amtToHeal);
            textBoxHandler.AddTextAsUseable(s.user.Id, Id);
        }
        battleStateMachine.ChangeState(BattleStates.BattleTextBox);
    }
    public override Useable ShallowClone()
    {
        return (SmallAllHeal)MemberwiseClone();
    }

    public override bool UseOnAll()
    {
        return true;
    }
    public override bool OnlyHeal()
    {
        return true;
    }
}
