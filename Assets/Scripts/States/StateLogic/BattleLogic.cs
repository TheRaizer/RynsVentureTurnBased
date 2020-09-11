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

    public Dictionary<EntityType, List<StatsManager>> AttackablesDic { get; set; } = new Dictionary<EntityType, List<StatsManager>>();

    public Enemy CurrentEnemy { get; private set; }
    public PlayableCharacter CurrentPlayer { get; private set; }
    public EntityAction CurrentPlayerAttack { get; set; }

    public PlayableCharacter[] ActivePlayableCharacters { get; set; } = new PlayableCharacter[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];//playable characters on field
    public List<PlayableCharacter> InactivePlayableCharacters { get; set; } = new List<PlayableCharacter>();//playable characters off field
    public List<PlayableCharacter> PlayableCharacterRoster { get; set; } = new List<PlayableCharacter>();//all playable characters


    private readonly MenusHandler menusHandler;
    public readonly StateMachine battleStateMachine;

    public BattleLogic(MenusHandler _menusHandler, StateMachine _battleStateMachine)
    {
        menusHandler = _menusHandler;
        battleStateMachine = _battleStateMachine;

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
            if (Enemies[i] != null)
            {
                if (!Enemies[i].GetComponent<Enemy>().Stats.HealthManager.Dead)
                {
                    EnemiesRemaining++;
                }
                else
                {
                    menusHandler.EnemyIdText[i].color = Color.white;

                    GameObject e = Enemies[i];
                    AttackablesDic[EntityType.Enemy].Remove(e.GetComponent<Enemy>().Stats);
                    Enemies[i] = null;

                    e.GetComponent<Enemy>().Stats.StatusEffectsManager.RemoveAllStatusEffects();

                    UnityEngine.Object.Destroy(e);
                    menusHandler.EnemyIdText[i].text = "Dead";
                }
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
                if (ActivePlayableCharacters[i] != null && !ActivePlayableCharacters[i].Stats.HealthManager.Dead)
                {
                    ActivePlayableCharacters[i].Stats.IncrementClockTick();

                    if (ActivePlayableCharacters[i].Stats.ClockTick >= CLOCK_TICK_MAX)
                    {
                        CurrentPlayer = ActivePlayableCharacters[i];
                        CurrentPlayer.Stats.ResetClockTick();
                        battleStateMachine.ChangeState(BattleStates.FightMenu);
                        return;
                    }
                }
            }

            for (int j = 0; j < Enemies.Length; j++)
            {
                if (Enemies[j] != null)
                {
                    StatsManager stats = Enemies[j].GetComponent<StatsManager>();

                    if (stats.HealthManager.Dead) continue;

                    stats.IncrementClockTick();

                    if (stats.ClockTick >= CLOCK_TICK_MAX)
                    {
                        CurrentEnemy = Enemies[j].GetComponent<Enemy>();
                        CurrentEnemy.Stats.ResetClockTick();
                        battleStateMachine.ChangeState(BattleStates.EnemyTurn);
                        return;
                    }
                }
            }
        }
    }

    public void CheckForAttackablePlayers()
    {
        for (int i = 0; i < ActivePlayableCharacters.Length; i++)
        {
            if (ActivePlayableCharacters[i] == null) continue;

            StatsManager currentPlayer = ActivePlayableCharacters[i].Stats;
            if (!currentPlayer.HealthManager.Dead && !AttackablesDic[EntityType.Player].Contains(currentPlayer))
            {
                if (AttackablesDic.TryGetValue(EntityType.Player, out List<StatsManager> stats))
                {
                    AttackablesDic[EntityType.Player].Add(currentPlayer);
                }
                else
                {
                    stats.Add(currentPlayer);
                    AttackablesDic.Add(EntityType.Player, stats);
                }
            }
            else if (AttackablesDic[EntityType.Player].Contains(currentPlayer) && currentPlayer.HealthManager.Dead)
            {
                AttackablesDic[EntityType.Player].Remove(currentPlayer);
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
