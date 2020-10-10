
public class FightCommand : BattleCommands
{
    private void Awake()
    {
        actionOnPress = Fight;
    }

    private void Fight(BattleHandler battleHandler)
    {
        battleHandler.BattleEntitiesManager.CurrentPlayerAttack = battleHandler.BattleEntitiesManager.CurrentPlayer.FightAttack;
        battleHandler.BattleStateMachine.ChangeState(BattleStates.EnemyChoice);
    }
}
