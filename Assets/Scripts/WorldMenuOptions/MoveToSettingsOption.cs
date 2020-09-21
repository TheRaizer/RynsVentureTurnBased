using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSettingsOption : MenuOption
{
    public override void OnSelection(StateMachine roamStateMachine)
    {
        base.OnSelection(roamStateMachine);

        roamStateMachine.ChangeState(WorldRoamingStates.SettingsState);
    }
}
