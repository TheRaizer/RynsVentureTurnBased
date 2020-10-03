using UnityEngine;

public class FightMenuState : StatusEffectCheckState
{
    private readonly BattleLogic battleLogic;
    private readonly BattleMenusHandler menusHandler;
    private readonly BattleStatusEffectsManager statusManager;
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly VectorMenuTraversal menuTraversal;

    public FightMenuState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleMenusHandler _menusHandler, BattleStatusEffectsManager _ailmentsManager, BattleTextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
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
        Debug.Log(battleLogic.CurrentPlayer.Id + " Turn");
        textBoxHandler.AddTextAsTurn(battleLogic.CurrentPlayer.Id);
        textBoxHandler.PreviousState = BattleStates.FightMenu;
        stateMachine.ChangeState(BattleStates.BattleTextBox);

        if (CheckForStatusEffects(statusManager, battleLogic, textBoxHandler, battleLogic.CurrentPlayer.Stats, BattleStates.FightMenu))
        {
            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }
        if(statusManager.CheckForReplacementStatusEffect(battleLogic, battleLogic.CurrentPlayer.Stats, false))
        {
            return;
        }

        battleLogic.CheckForEnemiesRemaining();
        if (battleLogic.EnemiesRemaining == 0)
        {
            stateMachine.ChangeState(BattleStates.Victory);
        }
        battleLogic.TextMods.PrintPlayerHealth();
        battleLogic.TextMods.PrintPlayerMana();
    }

    public override void InputUpdate()
    {
        base.LogicUpdate();

        menuTraversal.Traverse();

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            menusHandler.FightMenuCommands[menuTraversal.currentIndex].actionOnPress?.Invoke(battleLogic);
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
