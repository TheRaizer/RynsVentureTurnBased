using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : State
{
    private readonly BattleState battleState;
    private readonly BattleMenusHandler menusHandler;
    private readonly BattleHandler battleHandler;
    private readonly Inventory inventory;

    public VictoryState(StateMachine _stateMachine, BattleState _battleState, BattleMenusHandler _menusHandler, BattleHandler _battleHandler, Inventory _inventory) : base(_stateMachine)
    {
        battleState = _battleState;
        menusHandler = _menusHandler;
        battleHandler = _battleHandler;
        inventory = _inventory;
    }

    public override void OnFullRotationEnter()
    {
        base.OnEnterOrReturn();

        RemoveAllPlayerStatusEffects();
        EmptyEnemyTexts();
        battleHandler.CheckAllPlayerLevels();
        battleHandler.DestroyPlayerSprites();
        AddItemsWonToInventory();
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

    private void EmptyEnemyTexts()
    {
        for (int i = 0; i < menusHandler.EnemyIdText.Length; i++)
        {
            menusHandler.EnemyIdText[i].text = "";
        }
    }

    private void AddItemsWonToInventory()
    {
        for (int i = 0; i < battleHandler.ItemsToGiveToPlayer.Count; i++)
        {
            inventory.AddToInventory(battleHandler.ItemsToGiveToPlayer[i]);
        }
    }

    private void RemoveAllPlayerStatusEffects()
    {
        foreach (PlayableCharacter p in battleHandler.PlayableCharacterRoster)
        {
            if (p != null)
            {
                p.Stats.StatusEffectsManager.RemoveAllStatusEffects((StatusEffectAnimationState)stateMachine.states[BattleStates.StatusEffectAnimations]);
            }
        }
    }
}
