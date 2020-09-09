using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public IUser user;
    [field: SerializeField] public float DamageScale { get; set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int ClockTick { get; private set; }

    [SerializeField] private int baseMaxHp = 0;
    public HealthManager HealthManager { get; private set; }
    [field: SerializeField] public List<GameObject> MultiTurnTriggeredAilments { get; set; } = new List<GameObject>();
    [field: SerializeField] public GameObject TurnReplaceAilment { get; set; } = null;
    [field: SerializeField] public List<GameObject> SingleTurnTriggeredAilments { get; set; } = new List<GameObject>();

    public void Initialize(IUser _user)
    {
        HealthManager = new HealthManager(baseMaxHp);
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

    public void RemoveAllStatusEffects()
    {
        foreach(GameObject g in MultiTurnTriggeredAilments)
        {
            g.GetComponent<StatusEffect>().OnWornOff(this);
            Destroy(g);
        }
        foreach (GameObject g in SingleTurnTriggeredAilments)
        {
            g.GetComponent<StatusEffect>().OnWornOff(this);
            Destroy(g);
        }
        MultiTurnTriggeredAilments.Clear();
        SingleTurnTriggeredAilments.Clear();
        TurnReplaceAilment = null;
    }
}

