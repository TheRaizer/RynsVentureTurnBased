using UnityEngine;

public class FightMenuState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleMenusHandler menusHandler;
    private readonly BattleStatusEffectsManager statusManager;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly VectorMenuTraversal menuTraversal;

    public FightMenuState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleMenusHandler _menusHandler, BattleStatusEffectsManager _ailmentsManager, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        menusHandler = _menusHandler;
        statusManager = _ailmentsManager;
        textBoxHandler = _textBoxHandler;

        menuTraversal = new VectorMenuTraversal(PositionPointer)
        {
            MaxIndex = menusHandler.FightMenuCommands.Count - 1
        };
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        PositionPointer();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        menuTraversal.currentIndex = 0;
        Debug.Log(battleHandler.CurrentPlayer.Id + " Turn");
        PrepTextBox();
        MultiSingleStatusCheck();
        battleHandler.CheckForEnemiesRemaining();

        CheckIfWon();
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

    private void CheckIfWon()
    {
        if (battleHandler.EnemiesRemaining == 0)
        {
            stateMachine.ChangeState(BattleStates.Victory);
        }
    }

    private void ReplacementStatusCheck()
    {
        if (statusManager.CheckForReplacementStatusEffect(battleHandler, battleHandler.CurrentPlayer.Stats, false))
        {
            stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
        }
    }

    private void MultiSingleStatusCheck()
    {
        if (statusManager.CheckForStatusEffect(battleHandler, battleHandler.CurrentPlayer.Stats))
        {
            stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
        }
        else
        {
            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }
    }

    private void PrepTextBox()
    {
        textBoxHandler.AddTextAsTurn(battleHandler.CurrentPlayer.Id);
        textBoxHandler.PreviousState = BattleStates.FightMenu;
    }

    private void OnEnterSelected()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            menusHandler.FightMenuCommands[menuTraversal.currentIndex].actionOnPress?.Invoke(battleHandler);
        }
    }

    private void PositionPointer()
    {
        menusHandler.PositionPointer
            (
                menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation.top, 
                menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation.bottom, 
                menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation.left, 
                menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation.right
            );
    }
}
