public class PlayerChoiceState : State
{
    private readonly BattleMenusHandler battleMenusHandler;
    protected readonly VectorMenuTraversal vectorMenuTraversal;

    public PlayerChoiceState(StateMachine _stateMachine, BattleMenusHandler _battleMenusHandler) : base(_stateMachine)
    {
        battleMenusHandler = _battleMenusHandler;

        vectorMenuTraversal = new VectorMenuTraversal(PositionPointerForActivePlayers)
        {
            MaxIndex = ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS - 1
        };
    }
    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        PositionPointerForActivePlayers();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        vectorMenuTraversal.currentIndex = 0;
        PositionPointerForActivePlayers();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        vectorMenuTraversal.Traverse();
    }

    private void PositionPointerForActivePlayers()
    {
        battleMenusHandler.PositionPointer
            (
                battleMenusHandler.ActivePlayerPointerLocation[vectorMenuTraversal.currentIndex].top,
                battleMenusHandler.ActivePlayerPointerLocation[vectorMenuTraversal.currentIndex].bottom,
                battleMenusHandler.ActivePlayerPointerLocation[vectorMenuTraversal.currentIndex].left,
                battleMenusHandler.ActivePlayerPointerLocation[vectorMenuTraversal.currentIndex].right
            );
    }
}
