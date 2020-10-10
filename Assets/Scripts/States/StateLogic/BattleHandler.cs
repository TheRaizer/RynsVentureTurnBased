using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy
}

public class BattleHandler
{
    public BattleEntitiesManager BattleEntitiesManager { get; private set; }
    public BattleEntitySpritePositions BattleSpritePositions { private get; set; }
    public BattleEntitySprites BattleEntitySprites { get; private set; }
    public BattleMenusHandler MenusHandler { get; private set; }
    public TextModifications TextMods { get; }
    public StateMachine BattleStateMachine { get; }
    public BattleAnimationsHandler AnimationsHandler { get; }

    public Useable ItemToUse { get; set; }
    public int ItemIndex { get; set; }

    public BattleHandler(BattleMenusHandler _menusHandler, StateMachine _battleStateMachine)
    {
        MenusHandler = _menusHandler;
        BattleSpritePositions = MenusHandler.BattleEntitySpritePositions;
        BattleStateMachine = _battleStateMachine;
        AnimationsHandler = new BattleAnimationsHandler(_menusHandler);
        BattleEntitiesManager = new BattleEntitiesManager(MenusHandler, BattleStateMachine);
        BattleEntitySprites = new BattleEntitySprites(BattleEntitiesManager, BattleSpritePositions);
        TextMods = new TextModifications(MenusHandler, this);
    }

    public void SetActionPopupForEntity(GameObject user, List<GameObject> foesToAttack, GameObject foeToAttack = null, EntityActionInfo actionInfo = null,List<EntityActionInfo> actionInfos = null)
    {
        SpawnActionPopup spawnDamage = user.GetComponent<SpawnActionPopup>();

        List<int> amounts = new List<int>();
        List<bool> criticals = new List<bool>();
        List<bool> targetHits = new List<bool>();
        bool support;

        if (actionInfos != null)
        {
            support = actionInfos[0].support;
            if (foesToAttack.Count != actionInfos.Count)
            {
                Debug.LogError("When generating popups the number of foes to attack does not equal the number of foes attacked");
            }

            for (int i = 0; i < actionInfos.Count; i++)
            {
                amounts.Add(actionInfos[i].amount);
                criticals.Add(actionInfos[i].CriticalHit);
                targetHits.Add(actionInfos[i].hitTarget);
            }
        }
        else
        {
            support = actionInfo.support;
            amounts.Add(actionInfo.amount);
            criticals.Add(actionInfo.CriticalHit);
            targetHits.Add(actionInfo.hitTarget);
        }

        spawnDamage.Amount = amounts;
        spawnDamage.Critical = criticals;
        spawnDamage.Support = support;
        spawnDamage.TargetHits = targetHits;

        List<Vector2> locationsToSpawn = new List<Vector2>();

        if (foesToAttack == null)
        {
            RectTransform r = foeToAttack.GetComponent<RectTransform>();
            Vector2 pos = new Vector2(r.anchoredPosition.x, r.anchoredPosition.y);
            locationsToSpawn.Add(pos);
        }
        else
        {
            foreach (GameObject g in foesToAttack)
            {
                RectTransform r = g.GetComponent<RectTransform>();
                Vector2 pos = new Vector2(r.anchoredPosition.x, r.anchoredPosition.y);
                locationsToSpawn.Add(pos);
            }
        }
        spawnDamage.LocationsToSpawn = locationsToSpawn;
    }
}
