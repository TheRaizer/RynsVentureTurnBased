public class ItemCommand : BattleCommands
{
    private void Awake()
    {
        actionOnPress = ItemChoice;
    }

    private void ItemChoice(BattleLogic battleLogic)
    {
        battleLogic.BattleStateMachine.ChangeState(BattleStates.ItemChoice);
    }
}
