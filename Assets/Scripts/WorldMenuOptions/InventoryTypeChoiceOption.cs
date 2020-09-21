using UnityEngine;
using System;

[Serializable]
public class InventoryTypeChoiceOption : MonoBehaviour
{
    [field: SerializeField] public string OptionName { get; private set; }
    [SerializeField] private string inventoryTypeWord = "";

    private Type inventoryType;

    public void OnSelection(StateMachine roamStateMachine, Inventory inventory)
    {
        inventoryType = Type.GetType(inventoryTypeWord);
        Debug.Log(inventoryType);
        inventory.CurrentInventoryOpen = inventoryType;
        roamStateMachine.ChangeState(WorldRoamingStates.InventoryState);
    }
}
