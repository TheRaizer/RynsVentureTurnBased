using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour, ILevel, IUser
{
    [field: Header("Name")]
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public EntityType EntityType { get; private set; }

    [field: Header("Level Specifics")]
    [field: SerializeField] public float PercentHealthIncreasePerLevel { get; }
    [field: SerializeField] public float DamageScaleIncreasePerLevel { get; }
    [SerializeField] private int baseExperienceToNextLevel = 0;
    [SerializeField] private float percentExperiencePerLevel = 0;

    [field: Header ("Attacks")]
    [field: SerializeField] public EntityAction FightAttack { get; private set; }
    [field: SerializeField] public List<EntityAction> Magic { get; private set; } = new List<EntityAction>();

    [field: SerializeField] public LevelSystem LevelSystem { get; private set; }

    public StatsManager Stats { get; private set; }


    private void Awake()
    {
        LevelSystem = new LevelSystem(this, baseExperienceToNextLevel, percentExperiencePerLevel);
        Stats = GetComponent<StatsManager>();
        Stats.Initialize(this);
        if(Magic.Count > 12)
        {
            Debug.LogError("Magic actions exceeds limit");
        }
    }

    public void OnLevelUp()
    {
        Stats.HealthManager.IncreaseMaxAmount(PercentHealthIncreasePerLevel);
        Stats.DamageScale += DamageScaleIncreasePerLevel;
    }
}

public interface ILevel//health exponentially increase while damage scale linearly increases
{
    void OnLevelUp();
    float PercentHealthIncreasePerLevel { get; }
    float DamageScaleIncreasePerLevel { get; }//since damage scale is multiplied by each attacks damage the scale does not need to exponentially increase
}

public interface IUser
{
    string Id { get; }
    EntityType EntityType { get; }
}