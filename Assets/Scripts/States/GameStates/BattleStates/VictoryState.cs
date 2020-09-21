using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : State
{
    private readonly BattleState battleState;
    private readonly BattleMenusHandler menusHandler;
    private readonly BattleLogic battleLogic;

    public VictoryState(StateMachine _stateMachine, BattleState _battleState, BattleMenusHandler _menusHandler, BattleLogic _battleLogic) : base(_stateMachine)
    {
        battleState = _battleState;
        menusHandler = _menusHandler;
        battleLogic = _battleLogic;
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
