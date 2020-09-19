using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IUser
{
    [Header("Level Specifics")]
    [SerializeField] public int level = 0;
    [SerializeField] private float percentHealthIncrease = 0;//health exponentially increase while damage scale linearly increases
    [SerializeField] private float damageScalingIncrease = 0;//since damage scale is multiplied by each attacks damage the scale does not need to exponentially increase

    [field: Header("Name")]
    [field: SerializeField] public string Id { get; set; }
    [field: SerializeField] public EntityType EntityType { get; private set; }

    [field: SerializeField] public int ExpOnDeath { get; private set; }
    [field: SerializeField] public List<EntityAction> Attacks { get; private set; }
    [field: SerializeField] public List<GameObject> PrefabItemsToDrop { get; private set; }


    public StatsManager Stats { get; private set; }


    public void Awake()
    {
        Stats = GetComponent<StatsManager>();
        Stats.Initialize(this);

        Stats.HealthManager.IncreaseMaxAmount(percentHealthIncrease * level);
        Stats.DamageScale += damageScalingIncrease * level;
    }
}
