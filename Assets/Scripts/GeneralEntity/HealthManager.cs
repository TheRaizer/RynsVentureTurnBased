using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthManager
{
    public bool Dead { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }

    public HealthManager(int _maxHealth)
    {
        MaxHealth = _maxHealth;
        CurrentHealth = MaxHealth;
    }

    public void Hit(int amt)
    {
        CurrentHealth -= amt;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Dead = true;
        }
    }

    public void Heal(int amt)
    {
        CurrentHealth += amt;

        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
    public void InstaHeal()
    {
        CurrentHealth = MaxHealth;
    }
    public void InstaKill()
    {
        CurrentHealth = 0;
        Dead = true;
    }

    public void IncreaseMaxHealth(float percentIncrease)
    {
        MaxHealth += MathExtension.RoundToNearestInteger(MaxHealth * percentIncrease);
        InstaHeal();
    }

}
