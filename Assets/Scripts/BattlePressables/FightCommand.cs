
public class FightCommand : BattleCommands
{
    private void Awake()
    {
        actionOnPress = Fight;
    }

    private void Fight(BattleLogic battleLogic)
    {
        battleLogic.CurrentPlayerAttack = battleLogic.CurrentPlayer.FightAttack;
        battleLogic.BattleStateMachine.ChangeState(BattleStates.EnemyChoice);
    }
}
