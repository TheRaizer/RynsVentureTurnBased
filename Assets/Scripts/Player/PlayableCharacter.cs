using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour, ILevel, IUser
{
    [field: SerializeField] public GameObject BattleVersionPrefab { get; private set; }

    [field: Header("User Properties")]
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public EntityType EntityType { get; private set; }
    public Animator Animator { get; set; }

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
