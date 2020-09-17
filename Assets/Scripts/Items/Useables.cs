using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Useables : MonoBehaviour
{
    [SerializeField] protected string id;
    public int amount;
    public int MaxAmount { get; private set; }
    public bool IsEmpty => amount > 0;

    public virtual void OnUse(StatsManager statsManager, StateMachine battleStateMachine, TextBoxHandler textBoxHandler)
    {
        amount--;
    }

}
