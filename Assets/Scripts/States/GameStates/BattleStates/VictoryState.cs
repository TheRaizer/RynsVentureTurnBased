using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : State
{
    private readonly BattleState battleState;
    private readonly BattleMenusHandler menusHandler;
    private readonly BattleLogic battleLogic;
    private readonly Inventory inventory;

    public VictoryState(StateMachine _stateMachine, BattleState _battleState, BattleMenusHandler _menusHandler, BattleLogic _battleLogic, Inventory _inventory) : base(_stateMachine)
    {
        battleState = _battleState;
        menusHandler = _menusHandler;
        battleLogic = _battleLogic;
        inventory = _inventory;
    }

    public override void OnFullRotationEnter()
    {
        base.OnEnterOrReturn();

        foreach(PlayableCharacter p in battleLogic.PlayableCharacterRoster)
        {
            if (p != null)
            {
                p.Stats.StatusEffectsManager.RemoveAllStatusEffects();
            }
        }
        for (int i = 0; i < menusHandler.EnemyIdText.Length; i++)
        {
            menusHandler.EnemyIdText[i].text = "";
        }
        battleLogic.CheckAllPlayerLevels();
        for(int i = 0; i < battleLogic.ItemsToGiveToPlayer.Count; i++)
        {
            inventory.AddToInventory(battleLogic.ItemsToGiveToPlayer[i]);
        }
        Debug.Log("Victory");
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        //TEST CODE
        if(Input.GetKeyDown(KeyCode.E))
        {
            battleState.ChangeToWorldRoamState();
        }
    }
}
