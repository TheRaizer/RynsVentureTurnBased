using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicChoiceState : State
{
    private readonly BattleHandler battleHandler;
    private readonly MatrixMenuTraversal matrixMenuTraversal;
    private readonly BattleMenusHandler menusHandler;
    private readonly BattleTextBoxHandler textBoxHandler;

    private readonly EntityAction[,] magicAttacks = new EntityAction[3, 4];

    public MagicChoiceState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleMenusHandler _menusHandler, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        menusHandler = _menusHandler;
        textBoxHandler = _textBoxHandler;

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
        menusHandler.MagicPanel.SetActive(true);
        PositionPointerForMagic();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();
        for (int i = 0; i < menusHandler.MagicText.Length; i++)
        {
            menusHandler.MagicText[i].text = "";
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
        if (battleHandler.CurrentPlayerAttack.ActionType == EntityAction.ActionTypes.Attack)
        {
            stateMachine.ChangeState(BattleStates.EnemyChoice);
        }
        else if (battleHandler.CurrentPlayerAttack.ActionType == EntityAction.ActionTypes.Support || battleHandler.CurrentPlayerAttack.ActionType == EntityAction.ActionTypes.Revive)
        {
            stateMachine.ChangeState(BattleStates.SupportPlayerChoice);
        }
    }

    private void PositionPointerForMagic()
    {
        Directions pointerLocation = ArrayExtensions.Get1DElementFrom2DArray(menusHandler.MagicChoicePointerLocations, ConstantNumbers.MAX_MAGIC_X_LENGTH, matrixMenuTraversal.currentYIndex, matrixMenuTraversal.currentXIndex);
        menusHandler.PositionPointer(pointerLocation.top, pointerLocation.bottom, pointerLocation.left, pointerLocation.right);
    }

    public override void OnExit()
    {
        base.OnExit();

        menusHandler.MagicPanel.SetActive(false);
    }

    private void GenerateMagicText()
    {
        int playerMagicIndex = 0;
        for (int y = 0; y < ConstantNumbers.MAX_MAGIC_Y_LENGTH; y++)
        {
            for (int x = 0; x < ConstantNumbers.MAX_MAGIC_X_LENGTH; x++)
            {
                if (playerMagicIndex < battleHandler.CurrentPlayer.Magic.Count)
                {
                    EntityAction magicAction = battleHandler.CurrentPlayer.Magic[playerMagicIndex];

                    magicAttacks[x, y] = magicAction;
                    menusHandler.MagicText[playerMagicIndex].text = magicAction.Id;
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
            battleHandler.CurrentPlayerAttack = magicAttacks[matrixMenuTraversal.currentXIndex, matrixMenuTraversal.currentYIndex];

            if (!battleHandler.CurrentPlayer.Stats.ManaManager.CanUse(battleHandler.CurrentPlayerAttack.ManaReduction))
            {
                textBoxHandler.AddTextAsNotEnoughMana(battleHandler.CurrentPlayer.Id);
                textBoxHandler.PreviousState = BattleStates.MagicChoice;
                stateMachine.ChangeState(BattleStates.BattleTextBox);
                return;
            }

            if (!battleHandler.CurrentPlayerAttack.IsAOE)
            {
                SingleEntityAttack();
            }
            else
            {
                AOEState aoeState = (AOEState)stateMachine.states[BattleStates.AOEState];
                aoeState.ActionToUse = battleHandler.CurrentPlayerAttack;
                aoeState.EntityUsing = EntityType.Player;
                stateMachine.ChangeState(BattleStates.AOEState);
            }
        }
    }

}
