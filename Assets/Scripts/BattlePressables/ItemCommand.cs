public class ItemCommand : BattleCommands
{
    private void Awake()
    {
        actionOnPress = ItemChoice;
    }

    private void ItemChoice(BattleHandler battleHandler)
    {
        battleHandler.BattleStateMachine.ChangeState(BattleStates.ItemChoice);
    }
}
