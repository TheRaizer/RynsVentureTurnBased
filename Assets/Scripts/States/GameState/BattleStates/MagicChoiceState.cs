﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicChoiceState : State
{
    private readonly BattleLogic battleLogic;
    private readonly MatrixMenuTraversal matrixMenuTraversal;
    private readonly MenusHandler menusHandler;

    private readonly EntityAction[,] magicAttacks = new EntityAction[3, 4];
    private int playerMagicIndex = 0;

    public MagicChoiceState(StateMachine _stateMachine, BattleLogic _battleLogic, MenusHandler _menusHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        menusHandler = _menusHandler;

        matrixMenuTraversal = new MatrixMenuTraversal(PositionPointer)
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
        PositionPointer();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();
        for(int i = 0; i < menusHandler.MagicText.Length; i++)
        {
            menusHandler.MagicText[i].text = "";
        }

        for(int y = 0; y < ConstantNumbers.MAX_MAGIC_Y_LENGTH; y++)
        {
            for(int x = 0; x < ConstantNumbers.MAX_MAGIC_X_LENGTH; x++)
            {
                if (playerMagicIndex < battleLogic.CurrentPlayer.Magic.Count)
                {
                    EntityAction magicAction = battleLogic.CurrentPlayer.Magic[playerMagicIndex];

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
        playerMagicIndex = 0;
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        matrixMenuTraversal.Traverse(magicAttacks);

        if(Input.GetKeyDown(KeyCode.Return))
        {
            battleLogic.CurrentPlayerAttack = magicAttacks[matrixMenuTraversal.currentXIndex, matrixMenuTraversal.currentYIndex];
            if(!battleLogic.CurrentPlayerAttack.IsAOE)
            {
                stateMachine.ChangeState(BattleStates.EnemyChoice);
            }
            Debug.Log(battleLogic.CurrentPlayerAttack.Id);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.ReturnBackToState(BattleStates.FightMenu);
        }
    }

    private void PositionPointer()
    {
        Directions pointerLocation = ArrayExtensions.Get1DElementFrom2DArray(menusHandler.MagicChoicePointerLocations, ConstantNumbers.MAX_MAGIC_X_LENGTH, matrixMenuTraversal.currentYIndex, matrixMenuTraversal.currentXIndex);
        menusHandler.PositionPointer(pointerLocation.top, pointerLocation.bottom, pointerLocation.left, pointerLocation.right);
    }

    public override void OnExit()
    {
        base.OnExit();

        menusHandler.MagicPanel.SetActive(false);
    }
}
