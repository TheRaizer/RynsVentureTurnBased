
public class FightCommand : BattleCommands
{
    private void Awake()
    {
        actionOnPress = Fight;
    }

    private void Fight(BattleHandler battleHandler)
    {
        battleHandler.CurrentPlayerAttack = battleHandler.CurrentPlayer.FightAttack;
        battleHandler.BattleStateMachine.ChangeState(BattleStates.EnemyChoice);
    }
}
