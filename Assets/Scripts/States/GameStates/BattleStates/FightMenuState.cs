using System.Collections;
using UnityEngine;

public class FightMenuState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleStatusEffectsManager statusManager;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly VectorMenuTraversal menuTraversal;
    private readonly BattleEntitiesManager battleEntitiesManager;

    public FightMenuState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleStatusEffectsManager _ailmentsManager, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        statusManager = _ailmentsManager;
        textBoxHandler = _textBoxHandler;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;

        menuTraversal = new VectorMenuTraversal(PositionPointer)
        {
            MaxIndex = battleHandler.MenusHandler.FightMenuCommands.Count - 1
        };
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();
        battleHandler.MenusHandler.OpenPanels();
        PositionPointer();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();
        Debug.Log("changed too fightmenu state");
        menuTraversal.currentIndex = 0;
        Debug.Log(battleEntitiesManager.CurrentPlayer.Id + " Turn");
        PrepTextBox();
        MultiSingleStatusCheck();
        battleEntitiesManager.CheckForEnemiesRemaining();
        if(CheckIfWon()) return;
        if (battleEntitiesManager.CurrentPlayer.Stats.HealthManager.Dead)
        {
            battleEntitiesManager.CalculateNextTurn();
            return;
        }
        battleHandler.TextMods.PrintPlayerHealth();
        battleHandler.TextMods.ChangePlayerTextColour();
        battleHandler.TextMods.PrintPlayerMana();

        ReplacementStatusCheck();
    }

    public override void InputUpdate()
    {
        base.LogicUpdate();

        menuTraversal.Traverse();
        OnEnterSelected();
    }

    private bool CheckIfWon()
    {
        Debug.Log("Check if won");
        if (battleEntitiesManager.EnemiesRemaining == 0)
        {
            stateMachine.ChangeState(BattleStates.Victory);
            return true;
        }
        return false;
    }

    private void ReplacementStatusCheck()
    {
        Debug.Log("Check for replacement effect");
        if (statusManager.CheckForReplacementStatusEffect(battleHandler, battleEntitiesManager.CurrentPlayer.Stats, false))
        {
            stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
        }
    }

    private void MultiSingleStatusCheck()
    {
        textBoxHandler.PreviousState = BattleStates.FightMenu;
        Debug.Log("Check for status effect");
        if (statusManager.CheckForStatusEffect(battleHandler, battleEntitiesManager.CurrentPlayer.Stats))
        {
            battleEntitiesManager.CheckForEnemiesRemaining();

            StatusEffectAnimationState animState = (StatusEffectAnimationState)stateMachine.states[BattleStates.StatusEffectAnimations];
            if (!animState.CannotAnimateEffects)
            {
                stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
                return;
            }
        }
        stateMachine.ChangeState(BattleStates.BattleTextBox);
    }

    private void PrepTextBox()
    {
        Debug.Log("Prep Textbox");
        textBoxHandler.AddTextAsTurn(battleEntitiesManager.CurrentPlayer.Id);
    }

    private void OnEnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            battleHandler.MenusHandler.FightMenuCommands[menuTraversal.currentIndex].actionOnPress?.Invoke(battleHandler);
        }
    }

    private void PositionPointer()
    {
        BattleMenusHandler menusHandler = battleHandler.MenusHandler;

        menusHandler.PositionPointer(menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation);
    }
}
