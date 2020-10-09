using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleCommands : MonoBehaviour
{
    public Directions pointerLocation;
    public Action<BattleHandler> actionOnPress;
}
