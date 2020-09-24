using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Useable : Item
{
    public bool IsEmpty => Amount > 0;

    public virtual void OnUseInBattle(StatsManager user, List<StatsManager> friendlyStats, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        Amount--;
    }

    public virtual void OnUseInRoam(StatsManager statsManager, List<StatsManager> friendlyStats)
    {
        Amount--;
    }

    public virtual bool UseOnAll() { return false; }

    public virtual Useable ShallowClone()
    {
        return (Useable)MemberwiseClone();
    }
}
