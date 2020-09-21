using System.Collections;
using System.Collections.Generic;

public class MoveToInventoryOption : MenuOption
{
    public override void OnSelection(StateMachine roamStateMachine)
    {
        base.OnSelection(roamStateMachine);

        roamStateMachine.ChangeState(WorldRoamingStates.InventoryChoiceState);
    }
}
