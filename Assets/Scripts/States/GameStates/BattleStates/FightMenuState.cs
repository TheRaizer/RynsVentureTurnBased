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

        PositionPointer();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        menuTraversal.currentIndex = 0;
        Debug.Log(battleEntitiesManager.CurrentPlayer.Id + " Turn");
        PrepTextBox();
        MultiSingleStatusCheck();
        battleEntitiesManager.CheckForEnemiesRemaining();
        CheckIfWon();
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

    private void CheckIfWon()
    {
        if (battleEntitiesManager.EnemiesRemaining == 0)
        {
            stateMachine.ChangeState(BattleStates.Victory);
        }
    }

    private void ReplacementStatusCheck()
    {
        if (statusManager.CheckForReplacementStatusEffect(battleHandler, battleEntitiesManager.CurrentPlayer.Stats, false))
        {
            stateMachine.ChangeState(BattleStates.StatusEffectAnimations);
        }
    }

    private void MultiSingleStatusCheck()
    {
        if (statusManager.CheckForStatusEffect(battleHandler, battleEntitiesManager.CurrentPlayer.Stats))
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
        textBoxHandler.AddTextAsTurn(battleEntitiesManager.CurrentPlayer.Id);
        textBoxHandler.PreviousState = BattleStates.FightMenu;
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

        battleHandler.MenusHandler.PositionPointer
            (
                menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation.top,
                menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation.bottom,
                menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation.left,
                menusHandler.FightMenuCommands[menuTraversal.currentIndex].pointerLocation.right
            );
    }
}
