using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : StatusEffect//status effects like this one somehow need to print the action they did only on effect start and not every turn
{
    public override void OnEffectStart(StatsManager inhabitor)
    {
        base.OnEffectStart(inhabitor);

        inhabitor.DamageScale += 5;
    }

    public override void OnWornOff(StatsManager inhabitor)
    {
        base.OnWornOff(inhabitor);

        inhabitor.DamageScale -= 5;
    }

    public override StatusEffect ShallowCopy()
    {
        return (AttackUp)MemberwiseClone();
    }
}
