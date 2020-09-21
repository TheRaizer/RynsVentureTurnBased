using UnityEngine;

[System.Serializable]
public class MenuOption : MonoBehaviour
{
    [field: SerializeField] public string OptionName { get; private set; }
    public virtual void OnSelection(StateMachine roamStateMachine) { }
}
