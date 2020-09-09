using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLogic
{
    private const int CLOCK_TICK_MAX = 100;
    public int TotalExpFromBattle { get; set; } = 0;

    public int EnemiesRemaining { get; private set; }

    public GameObject[] Enemies { get; private set; } = new GameObject[ConstantNumbers.MAX_NUMBER_OF_ENEMIES];
    public List<StatsManager> AttackableEnemies { get; set; } = new List<StatsManager>();

    public Enemy CurrentEnemy { get; private set; }
    public PlayableCharacter CurrentPlayer { get; private set; }
    public Attack CurrentPlayerAttack { get; set; }

    public PlayableCharacter[] ActivePlayableCharacters { get; set; } = new PlayableCharacter[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];//playable characters on field
    public List<PlayableCharacter> InactivePlayableCharacters { get; set; } = new List<PlayableCharacter>();//playable characters off field
    public List<PlayableCharacter> PlayableCharacterRoster { get; set; } = new List<PlayableCharacter>();//all playable characters

    public readonly List<StatsManager> attackablePlayers = new List<StatsManager>();


    private readonly MenusHandler menusHandler;
    public readonly StateMachine battleStateMachine;

    public BattleLogic(MenusHandler _menusHandler, StateMachine _battleStateMachine)
    {
        menusHandler = _menusHandler;
        battleStateMachine = _battleStateMachine;
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
                    AttackableEnemies.Remove(e.GetComponent<Enemy>().Stats);
                    Enemies[i] = null;

                    RemoveStatusEffects(e.GetComponent<Enemy>().Stats);

                    Object.Destroy(e);
                    menusHandler.EnemyIdText[i].text = "Dead";
                }
            }
        }
    }

    public void RemoveStatusEffects(StatsManager stats)
    {
        for(int i = 0; i < stats.MultiTurnTriggeredAilments.Count; i++)
        {
            Object.Destroy(stats.MultiTurnTriggeredAilments[i]);
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
                        battleStateMachine.ChangeState(typeof(FightMenuState));
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
                        battleStateMachine.ChangeState(typeof(EnemyTurnState));
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

            if (!currentPlayer.HealthManager.Dead && !attackablePlayers.Contains(currentPlayer))
            {
                attackablePlayers.Add(currentPlayer);
            }
            else if (attackablePlayers.Contains(currentPlayer) && currentPlayer.HealthManager.Dead)
            {
                attackablePlayers.Remove(currentPlayer);
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
