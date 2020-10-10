using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : State
{
    private readonly BattleState battleState;
    private readonly BattleHandler battleHandler;
    private readonly Inventory inventory;
    private readonly BattleEntitiesManager battleEntitiesManager;

    public VictoryState(StateMachine _stateMachine, BattleState _battleState, BattleHandler _battleHandler, Inventory _inventory) : base(_stateMachine)
    {
        battleState = _battleState;
        battleHandler = _battleHandler;
        inventory = _inventory;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;
    }

    public override void OnFullRotationEnter()
    {
        base.OnEnterOrReturn();

        RemoveAllPlayerStatusEffects();
        EmptyEnemyTexts();
        battleEntitiesManager.CheckAllPlayerLevels();
        battleHandler.BattleEntitySprites.DestroyPlayerSprites();
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
        for (int i = 0; i < battleHandler.MenusHandler.EnemyIdText.Length; i++)
        {
            battleHandler.MenusHandler.EnemyIdText[i].text = "";
        }
    }

    private void AddItemsWonToInventory()
    {
        for (int i = 0; i < battleEntitiesManager.ItemsToGiveToPlayer.Count; i++)
        {
            inventory.AddToInventory(battleEntitiesManager.ItemsToGiveToPlayer[i]);
        }
    }

    private void RemoveAllPlayerStatusEffects()
    {
        foreach (PlayableCharacter p in battleEntitiesManager.PlayableCharacterRoster)
        {
            if (p != null)
            {
                p.Stats.StatusEffectsManager.RemoveAllStatusEffects((StatusEffectAnimationState)stateMachine.states[BattleStates.StatusEffectAnimations]);
            }
        }
    }
}
