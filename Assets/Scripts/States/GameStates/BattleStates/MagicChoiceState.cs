using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicChoiceState : State
{
    private readonly BattleHandler battleHandler;
    private readonly MatrixMenuTraversal matrixMenuTraversal;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleEntitiesManager battleEntitiesManager;

    private readonly EntityAction[,] magicAttacks = new EntityAction[3, 4];

    public MagicChoiceState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        textBoxHandler = _textBoxHandler;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;

        matrixMenuTraversal = new MatrixMenuTraversal(PositionPointerForMagic)
        {
            MaxXIndex = ConstantNumbers.MAX_MAGIC_X_LENGTH - 1,
            MaxYIndex = ConstantNumbers.MAX_MAGIC_Y_LENGTH - 1
        };
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();
        matrixMenuTraversal.ResetIndexes();
        battleHandler.MenusHandler.MagicPanel.SetActive(true);
        PositionPointerForMagic();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();
        for (int i = 0; i < battleHandler.MenusHandler.MagicText.Length; i++)
        {
            battleHandler.MenusHandler.MagicText[i].text = "";
        }

        GenerateMagicText();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        matrixMenuTraversal.TraverseWithNulls(magicAttacks);

        OnEnterSelection();
        OnExitSelection();
    }

    private void SingleEntityAttack()
    {
        EntityAction.ActionTypes actionType = battleEntitiesManager.CurrentPlayerAttack.ActionType;
        if (actionType == EntityAction.ActionTypes.Attack)
        {
            stateMachine.ChangeState(BattleStates.EnemyChoice);
        }
        else if (actionType == EntityAction.ActionTypes.Support || actionType == EntityAction.ActionTypes.Revive)
        {
            stateMachine.ChangeState(BattleStates.SupportPlayerChoice);
        }
    }

    private void PositionPointerForMagic()
    {
        Directions pointerLocation = ArrayExtensions.Get1DElementFrom2DArray
            (
            battleHandler.MenusHandler.MagicChoicePointerLocations, ConstantNumbers.MAX_MAGIC_X_LENGTH, matrixMenuTraversal.currentYIndex, matrixMenuTraversal.currentXIndex
            );
        battleHandler.MenusHandler.PositionPointer(pointerLocation.top, pointerLocation.bottom, pointerLocation.left, pointerLocation.right);
    }

    public override void OnExit()
    {
        base.OnExit();

        battleHandler.MenusHandler.MagicPanel.SetActive(false);
    }

    private void GenerateMagicText()
    {
        int playerMagicIndex = 0;
        for (int y = 0; y < ConstantNumbers.MAX_MAGIC_Y_LENGTH; y++)
        {
            for (int x = 0; x < ConstantNumbers.MAX_MAGIC_X_LENGTH; x++)
            {
                if (playerMagicIndex < battleEntitiesManager.CurrentPlayer.Magic.Count)
                {
                    EntityAction magicAction = battleEntitiesManager.CurrentPlayer.Magic[playerMagicIndex];

                    magicAttacks[x, y] = magicAction;
                    battleHandler.MenusHandler.MagicText[playerMagicIndex].text = magicAction.Id;
                    playerMagicIndex++;
                }
                else
                {
                    magicAttacks[x, y] = null;
                }
            }
        }
    }

    private void OnExitSelection()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ReturnBackToState(BattleStates.FightMenu);
        }
    }

    private void OnEnterSelection()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            battleEntitiesManager.CurrentPlayerAttack = magicAttacks[matrixMenuTraversal.currentXIndex, matrixMenuTraversal.currentYIndex];

            if (!battleEntitiesManager.CurrentPlayer.Stats.ManaManager.CanUse(battleEntitiesManager.CurrentPlayerAttack.ManaReduction))
            {
                textBoxHandler.AddTextAsNotEnoughMana(battleEntitiesManager.CurrentPlayer.Id);
                textBoxHandler.PreviousState = BattleStates.MagicChoice;
                stateMachine.ChangeState(BattleStates.BattleTextBox);
                return;
            }

            if (!battleEntitiesManager.CurrentPlayerAttack.IsAOE)
            {
                SingleEntityAttack();
            }
            else
            {
                AOEState aoeState = (AOEState)stateMachine.states[BattleStates.AOEState];
                aoeState.ActionToUse = battleEntitiesManager.CurrentPlayerAttack;
                aoeState.EntityUsing = EntityType.Player;
                stateMachine.ChangeState(BattleStates.AOEState);
            }
        }
    }

}
