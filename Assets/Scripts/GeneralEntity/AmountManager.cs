using UnityEngine;

[System.Serializable]
public class AmountManager
{
    [field: SerializeField] public int CurrentAmount { get; protected set; }
    [field: SerializeField] public int MaxAmount { get; protected set; }

    public AmountManager(int maxAmount)
    {
        MaxAmount = maxAmount;
        CurrentAmount = MaxAmount;
    }
    public virtual void ReduceAmount(int amt)
    {
        CurrentAmount -= amt;
    }

    public virtual void RegenAmount(int amt)
    {
        CurrentAmount += amt;

        if (CurrentAmount > MaxAmount)
        {
            CurrentAmount = MaxAmount;
        }
    }
    public virtual void MaxRegen()
    {
        CurrentAmount = MaxAmount;
    }
    public virtual void ZeroOut()
    {
        CurrentAmount = 0;
    }

    public virtual void IncreaseMaxAmount(float percentIncrease)
    {
        MaxAmount += MathExtension.RoundToNearestInteger(MaxAmount * percentIncrease);
        MaxRegen();
    }
}
