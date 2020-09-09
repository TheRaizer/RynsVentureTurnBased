using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenusHandler : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private List<GameObject> objectsToDisableOnTextOpen = null;

    [field: Header("Text Area")]
    [field: SerializeField] public TextMeshProUGUI TextArea { get; private set; }
    [SerializeField] private GameObject textPanel = null;

    [field: Header("Battle Commands")]
    [field: SerializeField] public List<BattleCommands> FightMenuCommands { get; private set; }

    [field: Header("Pointer Locations")]
    [field: SerializeField] public Directions[] EnemyChoicePointerLocations { get; private set; } = new Directions[ConstantNumbers.MAX_NUMBER_OF_ENEMIES];

    [field: Header("Panel Texts")]
    [field: SerializeField] public TextMeshProUGUI[] EnemyIdText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_NUMBER_OF_ENEMIES];
    [field: SerializeField] public TextMeshProUGUI[] PlayerHealthText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];
    [field: SerializeField] public TextMeshProUGUI[] PlayerNameText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];
    [field: SerializeField] public TextMeshProUGUI[] PlayerManaText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];

    [field: Header("Pointer")]
    [field: SerializeField] public Image MenuPointer { get; set; }

    public void PositionPointer(float top, float bottom, float left, float right)
    {
        RectTransformExtensions.SetTop(MenuPointer.rectTransform, top);
        RectTransformExtensions.SetBottom(MenuPointer.rectTransform, bottom);
        RectTransformExtensions.SetLeft(MenuPointer.rectTransform, left);
        RectTransformExtensions.SetRight(MenuPointer.rectTransform, right);
    }

    public void OpenTextPanel()
    {
        foreach(GameObject g in objectsToDisableOnTextOpen)
        {
            g.SetActive(false);
        }
        textPanel.SetActive(true);
    }

    public void CloseTextPanel()
    {
        textPanel.SetActive(false);
        foreach(GameObject g in objectsToDisableOnTextOpen)
        {
            g.SetActive(true);
        }
    }
}