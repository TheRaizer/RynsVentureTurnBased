using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public IUser user;
    [field: SerializeField] public float DamageScale { get; set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int ClockTick { get; private set; }

    [SerializeField] private int baseMaxHp = 0;
    [SerializeField] private int baseMaxMp = 0;
    [field: SerializeField] public HealthManager HealthManager { get; private set; }
    [field: SerializeField] public ManaManager ManaManager { get; private set; }

    public StatusEffectsManager StatusEffectsManager { get; private set; }

    public void Initialize(IUser _user)
    {
        HealthManager = new HealthManager(baseMaxHp);
        ManaManager = new ManaManager(baseMaxMp);
        StatusEffectsManager = new StatusEffectsManager(this);
        user = _user;
    }

    public void IncrementClockTick()
    {
        ClockTick += Speed;
    }

    public void ResetClockTick()
    {
        ClockTick = 0;
    }
}

