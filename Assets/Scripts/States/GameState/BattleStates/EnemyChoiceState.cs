using UnityEngine;

public class EnemyChoiceState : State
{
    private readonly BattleLogic battleLogic;
    private readonly BattleMenusHandler menusHandler;
    public readonly VectorMenuTraversal menuTraversal;
    private readonly TextModifications textMods;
    private readonly TextBoxHandler textBoxHandler;
    private readonly BattleStatusEffectsManager battleStatusManager;

    public EnemyChoiceState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleMenusHandler _menusHandler, TextModifications _textMods, TextBoxHandler _textBoxHandler, BattleStatusEffectsManager _battleStatusManager) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        menusHandler = _menusHandler;
        textMods = _textMods;
        textBoxHandler = _textBoxHandler;
        battleStatusManager = _battleStatusManager;

        menuTraversal = new VectorMenuTraversal(PositionPointer)
        {
            MaxIndex = battleLogic.Enemies.Length - 1
        };
    }

    
    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        while (battleLogic.Enemies[menuTraversal.currentIndex] == null)
        {
            menuTraversal.currentIndex++;
            menuTraversal.CheckIfIndexInRange();
        }
        PositionPointer();
    }

    public override void OnFullRotationEnter()
    {
        base.OnFullRotationEnter();

        if (battleStatusManager.CheckForReplacementStatusEffect(battleLogic, battleLogic.CurrentPlayer.Stats, true))
        {
            return;
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        menuTraversal.Traverse(battleLogic.Enemies);
        EnterChoice();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            stateMachine.ReturnBackToState(BattleStates.FightMenu);
        }
    }

    private void EnterChoice()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Enemy enemyToAttack = battleLogic.Enemies[menuTraversal.currentIndex].GetComponent<Enemy>();
            EntityActionInfo attackInfo = battleLogic.CurrentPlayerAttack.UseAttack(enemyToAttack.Stats, battleLogic.CurrentPlayer.Stats.DamageScale, textBoxHandler);

            textBoxHandler.AddTextAsAttack(battleLogic.CurrentPlayer.Id, battleLogic.CurrentPlayerAttack.AttackText, enemyToAttack.Id);

            if (!attackInfo.hitTarget)
            {
                textBoxHandler.AddTextOnMiss(battleLogic.CurrentPlayer.Id, attackInfo.targetId);
            }
            else if(battleLogic.CurrentPlayerAttack.WasCriticalHit)
            {
                textBoxHandler.AddTextAsCriticalHit();
            }

            battleLogic.CheckForEnemiesRemaining();
            textMods.ChangeEnemyNameColour();

            Debug.Log("printing player attack");
            stateMachine.ChangeState(BattleStates.BattleTextBox);
        }
    }

    private void PositionPointer()
    {
        menusHandler.PositionPointer
            (
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].top,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].bottom,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].left,
                menusHandler.EnemyChoicePointerLocations[menuTraversal.currentIndex].right
            );
    }
}
