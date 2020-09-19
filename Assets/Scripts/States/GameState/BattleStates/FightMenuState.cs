using UnityEngine;

public class FightMenuState : StatusEffectCheckState
{
    private readonly BattleLogic battleLogic;
    private readonly BattleMenusHandler menusHandler;
    private readonly BattleStatusEffectsManager statusManager;
    private readonly TextBoxHandler textBoxHandler;

    private int actionChoiceIndex = 0;

    public override bool CheckedStatusEffectThisTurn { get; set; }

    public FightMenuState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleMenusHandler _menusHandler, BattleStatusEffectsManager _ailmentsManager, TextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        menusHandler = _menusHandler;
        statusManager = _ailmentsManager;
        textBoxHandler = _textBoxHandler;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        actionChoiceIndex = 0;
        PositionPointer();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        Debug.Log(battleLogic.CurrentPlayer.Id + " Turn");
        if (CheckForStatusEffects(statusManager, battleLogic, textBoxHandler, battleLogic.CurrentPlayer.Stats, BattleStates.FightMenu))
        {
            Debug.Log("print status effects");
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
    }

    public override void InputUpdate()
    {
        base.LogicUpdate();

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            actionChoiceIndex--;
            CheckIfIndexInRange();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            actionChoiceIndex++;
            CheckIfIndexInRange();
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            menusHandler.FightMenuCommands[actionChoiceIndex].actionOnPress?.Invoke(battleLogic);
        }
    }

    private void CheckIfIndexInRange()
    {
        if (actionChoiceIndex >= menusHandler.FightMenuCommands.Count)
            actionChoiceIndex = 0;
        else if (actionChoiceIndex < 0)
            actionChoiceIndex = menusHandler.FightMenuCommands.Count - 1;
        PositionPointer();
    }

    private void PositionPointer()
    {
        menusHandler.PositionPointer
            (
                menusHandler.FightMenuCommands[actionChoiceIndex].pointerLocation.top, 
                menusHandler.FightMenuCommands[actionChoiceIndex].pointerLocation.bottom, 
                menusHandler.FightMenuCommands[actionChoiceIndex].pointerLocation.left, 
                menusHandler.FightMenuCommands[actionChoiceIndex].pointerLocation.right
            );
    }
}
