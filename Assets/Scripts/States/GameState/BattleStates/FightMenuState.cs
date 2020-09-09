using UnityEngine;

public class FightMenuState : StatusEffectCheckState
{
    private readonly BattleLogic battleLogic;
    private readonly MenusHandler menusHandler;
    private readonly StatusEffectsManager ailmentsManager;
    private readonly TextBoxHandler textBoxHandler;

    private int actionChoiceIndex = 0;

    public override bool CheckedStatusEffectThisTurn { get; set; }

    public FightMenuState(StateMachine _stateMachine, BattleLogic _battleLogic, MenusHandler _menusHandler, StatusEffectsManager _ailmentsManager, TextBoxHandler _textBoxHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        menusHandler = _menusHandler;
        ailmentsManager = _ailmentsManager;
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

        if (CheckForStatusEffects(ailmentsManager, battleLogic, textBoxHandler, battleLogic.CurrentPlayer.Stats, this))
        {
            Debug.Log("Checked status effects");
            stateMachine.ChangeState(typeof(BattleTextBoxState));
        }
        if(ailmentsManager.CheckForReplacementStatusEffect(battleLogic.attackablePlayers, battleLogic.AttackableEnemies, battleLogic.CurrentPlayer.Stats))
        {
            return;
        }
        Debug.Log("Player Turn");

        battleLogic.CheckForEnemiesRemaining();
        if (battleLogic.EnemiesRemaining == 0)
        {
            stateMachine.ChangeState(typeof(VictoryState));
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
