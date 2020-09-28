using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy
}

public class BattleLogic
{
    private const int CLOCK_TICK_MAX = 100;
    public int TotalExpFromBattle { get; set; } = 0;

    public int EnemiesRemaining { get; private set; }

    public GameObject[] Enemies { get; private set; } = new GameObject[ConstantNumbers.MAX_NUMBER_OF_ENEMIES];

    public Dictionary<EntityType, List<StatsManager>> AttackablesDic { get; set; } = new Dictionary<EntityType, List<StatsManager>>();//attackableEnemies is only used to track if player wins

    public Enemy CurrentEnemy { get; private set; }
    public PlayableCharacter CurrentPlayer { get; private set; }
    public EntityAction CurrentPlayerAttack { get; set; }
    public PlayableCharacter[] ActivePlayableCharacters { get; set; } = new PlayableCharacter[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];//playable characters on field
    public List<PlayableCharacter> InactivePlayableCharacters { get; set; } = new List<PlayableCharacter>();//playable characters off field
    public List<PlayableCharacter> PlayableCharacterRoster { get; set; } = new List<PlayableCharacter>();//all playable characters


    private readonly BattleMenusHandler menusHandler;
    public readonly TextModifications textMods;
    public StateMachine BattleStateMachine { get; }

    public Useable ItemToUse { get; set; }
    public int ItemIndex { get; set; }

    public BattleLogic(BattleMenusHandler _menusHandler, StateMachine _battleStateMachine)
    {
        menusHandler = _menusHandler;
        BattleStateMachine = _battleStateMachine;
        textMods = new TextModifications(menusHandler, this);
        foreach (EntityType a in Enum.GetValues(typeof(EntityType)))
        {
            AttackablesDic.Add(a, new List<StatsManager>());
        }
    }

    public void CheckForEnemiesRemaining()
    {
        EnemiesRemaining = 0;

        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] == null) continue;

            if (Enemies[i].GetComponent<Enemy>().Stats.HealthManager.Dead)
            {
                menusHandler.EnemyIdText[i].color = Color.white;

                GameObject e = Enemies[i];
                AttackablesDic[EntityType.Enemy].Remove(e.GetComponent<Enemy>().Stats);
                Enemies[i] = null;

                e.GetComponent<Enemy>().Stats.StatusEffectsManager.RemoveAllStatusEffects();

                UnityEngine.Object.Destroy(e);
                menusHandler.EnemyIdText[i].text = "Dead";
            }
            else
            {
                EnemiesRemaining++;
            }
        }
    }

    public void ResetAllPlayerClockTicks()
    {
        foreach(PlayableCharacter player in PlayableCharacterRoster)
        {
            player.Stats.ResetClockTick();
        }
    }

    public void CalculateNextTurn()
    {
        while (true)
        {
            for (int i = 0; i < ActivePlayableCharacters.Length; i++)
            {
                if (ActivePlayableCharacters[i] == null || ActivePlayableCharacters[i].Stats.HealthManager.Dead) continue;
                ActivePlayableCharacters[i].Stats.IncrementClockTick();
                if (ActivePlayableCharacters[i].Stats.ClockTick < CLOCK_TICK_MAX) continue;

                CurrentPlayer = ActivePlayableCharacters[i];
                CurrentPlayer.Stats.ResetClockTick();
                BattleStateMachine.ChangeState(BattleStates.FightMenu);
                return;
            }

            for (int j = 0; j < Enemies.Length; j++)
            {
                if (Enemies[j] == null) continue;
                StatsManager stats = Enemies[j].GetComponent<StatsManager>();
                if (stats.HealthManager.Dead) continue;

                stats.IncrementClockTick();
                if (stats.ClockTick < CLOCK_TICK_MAX) continue;

                CurrentEnemy = Enemies[j].GetComponent<Enemy>();
                CurrentEnemy.Stats.ResetClockTick();
                BattleStateMachine.ChangeState(BattleStates.EnemyTurn);
                return;
            }
        }
    }

    public void CheckForAttackablePlayers()
    {
        for (int i = 0; i < ActivePlayableCharacters.Length; i++)
        {
            if (ActivePlayableCharacters[i] == null) continue;

            StatsManager currentPlayer = ActivePlayableCharacters[i].Stats;
            if (currentPlayer.HealthManager.Dead && AttackablesDic[EntityType.Player].Contains(currentPlayer))
            {
                AttackablesDic[EntityType.Player].Remove(currentPlayer);
            }
            else
            {
                if (AttackablesDic.TryGetValue(EntityType.Player, out List<StatsManager> stats))
                {
                    if (!AttackablesDic[EntityType.Player].Contains(currentPlayer))
                    {
                        AttackablesDic[EntityType.Player].Add(currentPlayer);
                    }
                }
                else
                {
                    stats.Add(currentPlayer);
                    AttackablesDic.Add(EntityType.Player, stats);
                }
            }
        }
    }

    public void CheckAllPlayerLevels()
    {
        foreach(PlayableCharacter p in PlayableCharacterRoster)
        {
            p.LevelSystem.Experience += TotalExpFromBattle;
            p.LevelSystem.CheckLevel();
        }

        TotalExpFromBattle = 0;
    }
}
