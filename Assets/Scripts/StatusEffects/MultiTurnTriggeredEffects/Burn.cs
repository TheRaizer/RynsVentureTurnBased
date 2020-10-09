using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : StatusEffect
{
    [SerializeField] private float percentDiminish = 0.12f;

    public override void OnTurn(BattleHandler battleLogic, StatsManager infectee, StateMachine battleStateMachine, BattleTextBoxHandler textBoxHandler)
    {
        base.OnTurn(battleLogic, infectee, battleStateMachine, textBoxHandler);

        int amtToHit = MathExtension.RoundToNearestInteger(infectee.HealthManager.MaxAmount * percentDiminish);
        if (amtToHit == 0) amtToHit = 1;

        infectee.HealthManager.ReduceAmount(amtToHit);
    }

    public override StatusEffect ShallowCopy()
    {
        return (Burn)MemberwiseClone();
    }
}
