using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : StatusEffect
{
    public override void OnEffectStart(StatsManager inhabitor, TextBoxHandler textBoxHandler)
    {
        base.OnEffectStart(inhabitor, textBoxHandler);
        textBoxHandler.AddTextAsStatusEffect(inhabitor.user.Id, Name);
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
