using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthManager : AmountManager
{
    public bool Dead { get; set; }

    public HealthManager(int _maxHealth) : base(_maxHealth)
    {
    }

    public override void ReduceAmount(int amt)
    {
        base.ReduceAmount(amt);

        if(CurrentAmount <= 0)
        {
            CurrentAmount = 0;
            Dead = true;
        }
    }

    public override void ZeroOut()
    {
        base.ZeroOut();
        Dead = true;
    }

}
