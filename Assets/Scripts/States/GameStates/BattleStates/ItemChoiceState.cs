using System.Collections.Generic;
using UnityEngine;

public class ItemChoiceState : State
{
    private readonly Inventory inventory;
    private readonly BattleHandler battleHandler;
    private readonly VectorMenuTraversal itemTraversal;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleMenusHandler menusHandler;

    public ItemChoiceState(StateMachine _stateMachine, Inventory _inventory, BattleHandler _battleHandler, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        inventory = _inventory;
        battleHandler = _battleHandler;
        textBoxHandler = _textBoxHandler;
        menusHandler = battleHandler.MenusHandler;
        itemTraversal = new VectorMenuTraversal(PositionPointerForItemUse);
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        menusHandler.ItemChoicePanel.SetActive(true);
        PositionPointerForItemUse();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        inventory.CurrentInventoryOpen = typeof(Useable);
        menusHandler.EmptyItemTextBoxes();
        inventory.PrintCurrentInventoryText(menusHandler.ItemTextBoxes);
        itemTraversal.MaxIndex = inventory.InventoryDic[inventory.CurrentInventoryOpen].Count - 1;
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        itemTraversal.Traverse();

        CheckIfEnterSelected();
        CheckIfExitSelected();
    }

    public override void OnExit()
    {
        base.OnExit();

        menusHandler.ItemChoicePanel.SetActive(false);
    }

    private void CheckIfExitSelected()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.ReturnBackToState(BattleStates.FightMenu);
        }
    }

    private void CheckIfEnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            battleHandler.ItemIndex = itemTraversal.currentIndex;
            Useable useable = (Useable)inventory.InventoryDic[inventory.CurrentInventoryOpen][battleHandler.ItemIndex];
            battleHandler.ItemToUse = useable;

            if (useable.UseOnAll())
            {
                List<StatsManager> friendlyStats = new List<StatsManager>();
                foreach (PlayableCharacter p in battleHandler.BattleEntitiesManager.ActivePlayableCharacters)
                {
                    friendlyStats.Add(p.Stats);
                }
                useable.OnUseInBattle(battleHandler.BattleEntitiesManager.CurrentPlayer.Stats, friendlyStats, stateMachine, textBoxHandler);
            }
            else if (!useable.UseOnAll())
            {
                stateMachine.ChangeState(BattleStates.ItemPlayerChoice);
            }
        }
    }

    private void PositionPointerForItemUse()
    {
        menusHandler.PositionPointer(menusHandler.ItemUsePointerLocations[itemTraversal.currentIndex]);
    }
}
