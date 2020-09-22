using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Useable : Item
{
    public bool IsEmpty => Amount > 0;

    public virtual void OnUseInBattle(StatsManager statsManager, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        Amount--;
    }

    public virtual void OnUseInWorld(StatsManager statsManager)
    {
        Amount--;
    }

    public virtual Useable ShallowClone()
    {
        return (Useable)MemberwiseClone();
    }
}
