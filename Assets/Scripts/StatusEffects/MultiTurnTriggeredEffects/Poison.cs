using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    [SerializeField] private float percentDiminish = 0.07f;

    public override void OnTurn(BattleLogic battleLogic, StatsManager infectee, StateMachine battleStateMachine, TextBoxHandler textBoxHandler)
    {
        base.OnTurn(battleLogic, infectee, battleStateMachine, textBoxHandler);

        int amtToHit = MathExtension.RoundToNearestInteger(infectee.HealthManager.MaxAmount * percentDiminish);
        if (amtToHit == 0) amtToHit = 1;

        infectee.HealthManager.ReduceAmount(amtToHit);
    }

    public override StatusEffect ShallowCopy()
    {
        return (Poison)MemberwiseClone();
    }
}