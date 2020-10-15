using UnityEngine;

public class BattleTextBoxState : State
{
    private readonly BattleTextBoxHandler textBoxHandler;
    private readonly BattleMenusHandler menusHandler;

    public BattleTextBoxState(StateMachine _stateMachine, BattleTextBoxHandler _textBoxHandler, BattleHandler _battleHandler) : base(_stateMachine)
    {
        textBoxHandler = _textBoxHandler;
        menusHandler = _battleHandler.MenusHandler;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();
        if (textBoxHandler.NoTextToOutput)
        {
            textBoxHandler.ReturnOrCalculateNextState();
        }
        menusHandler.OpenTextPanel();
        menusHandler.StartCoroutine(textBoxHandler.BuildMultiStringTextCo());
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        textBoxHandler.InputUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();

        menusHandler.CloseTextPanel();
    }
}
