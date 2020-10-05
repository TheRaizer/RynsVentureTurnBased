using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy
}

public class BattleLogic
{
    private const int CLOCK_TICK_MAX = 100;
    public List<Item> ItemsToGiveToPlayer { get; set; } = new List<Item>();
    public int TotalExpFromBattle { get; set; } = 0;

    public int EnemiesRemaining { get; private set; }//when this hits zero the player wins

    public GameObject[] Enemies { get; private set; } = new GameObject[ConstantNumbers.MAX_NUMBER_OF_ENEMIES];//the number of enemies on the field. It cannot surpass a given limit

    public Dictionary<EntityType, List<StatsManager>> AttackablesDic { get; set; } = new Dictionary<EntityType, List<StatsManager>>();//attackableEnemies is only used to track if player wins

    public Enemy CurrentEnemy { get; private set; }
    public PlayableCharacter CurrentPlayer { get; private set; }
    public EntityAction CurrentPlayerAttack { get; set; }
    public PlayableCharacter[] ActivePlayableCharacters { get; set; } = new PlayableCharacter[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];//playable characters on field
    public List<PlayableCharacter> InactivePlayableCharacters { get; set; } = new List<PlayableCharacter>();//playable characters off field
    public List<PlayableCharacter> PlayableCharacterRoster { get; set; } = new List<PlayableCharacter>();//all playable characters


    private readonly BattleMenusHandler menusHandler;
    public TextModifications TextMods { get; }
    public StateMachine BattleStateMachine { get; }
    public BattleAnimationsHandler AnimationsHandler { get; }

    public Useable ItemToUse { get; set; }
    public int ItemIndex { get; set; }

    public BattleLogic(BattleMenusHandler _menusHandler, StateMachine _battleStateMachine)
    {
        menusHandler = _menusHandler;
        BattleStateMachine = _battleStateMachine;
        TextMods = new TextModifications(menusHandler, this);
        AnimationsHandler = new BattleAnimationsHandler(_menusHandler);
        foreach (EntityType a in Enum.GetValues(typeof(EntityType)))
        {
            AttackablesDic.Add(a, new List<StatsManager>());
        }
    }

    public void OutputActivePlayerSprites()
    {
        for(int i = 0; i < ActivePlayableCharacters.Length; i++)
        {
            if(ActivePlayableCharacters[i] != null)
            {
                GameObject g = UnityEngine.Object.Instantiate(ActivePlayableCharacters[i].BattleVersionPrefab);
                ActivePlayableCharacters[i].Animator = g.GetComponent<Animator>();
                g.transform.SetParent(menusHandler.Entities.transform);
                RectTransform rect = g.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(menusHandler.PlayerSpritesReferenceVector.x, menusHandler.PlayerSpritesReferenceVector.y);
                menusHandler.PlayerSpritesReferenceVector.y -= ConstantNumbers.SPACE_BETWEEN_ENTITIES;
            }
        }
    }

    public void OutputEnemySprites()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null)
            {
                Enemy enemy = Enemies[i].GetComponent<Enemy>();
                GameObject g = UnityEngine.Object.Instantiate(enemy.BattleVersionPrefab);
                enemy.Animator = g.GetComponent<Animator>();
                g.transform.SetParent(menusHandler.Entities.transform);
                RectTransform rect = g.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(menusHandler.EnemySpritesReferenceVector.x, menusHandler.EnemySpritesReferenceVector.y);
                menusHandler.EnemySpritesReferenceVector.y -= ConstantNumbers.SPACE_BETWEEN_ENTITIES;
            }
        }
    }
    public void CheckForItemDrop(Enemy enemy)
    {
        int dropChance = UnityEngine.Random.Range(0, 100);

        for(int i = 0; i < enemy.DroppableItems.Count; i++)
        {
            if(dropChance < enemy.DropChance[i])
            {
                ItemsToGiveToPlayer.Add(enemy.DroppableItems[i]);
            }
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

                Enemy enemy = e.GetComponent<Enemy>();
                enemy.Stats.StatusEffectsManager.RemoveAllStatusEffects();
                CheckForItemDrop(enemy);
                UnityEngine.Object.Destroy(enemy.Animator.gameObject);

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

    public void SetActionPopupForEntity(GameObject user, List<GameObject> foesToAttack, int damage, bool critical, bool support, bool missed)
    {
        SpawnActionPopup spawnDamage = user.GetComponent<SpawnActionPopup>();
        spawnDamage.Amount = damage;
        spawnDamage.Critical = critical;
        spawnDamage.Support = support;
        spawnDamage.HitTarget = missed;

        List<Vector2> locationsToSpawn = new List<Vector2>();

        foreach(GameObject g in foesToAttack)
        {
            RectTransform r = g.GetComponent<RectTransform>();
            Vector2 pos = new Vector2(r.anchoredPosition.x, r.anchoredPosition.y);
            locationsToSpawn.Add(pos);
        }
        spawnDamage.LocationsToSpawn = locationsToSpawn;
    }
}
