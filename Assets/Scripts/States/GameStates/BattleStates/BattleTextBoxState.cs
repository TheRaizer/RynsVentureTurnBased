using UnityEngine;

public class BattleTextBoxState : State
{
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleMenusHandler menuHandler;

    public BattleTextBoxState(StateMachine _stateMachine, BattleTextBoxHandler _textBoxHandler, BattleMenusHandler _menuHandler) : base(_stateMachine)
    {
        textBoxHandler = _textBoxHandler;
        menuHandler = _menuHandler;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        menuHandler.OpenTextPanel();
        menuHandler.StartCoroutine(textBoxHandler.BuildMultiStringTextCo());
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        textBoxHandler.InputUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        menuHandler.CloseTextPanel();
    }
}
