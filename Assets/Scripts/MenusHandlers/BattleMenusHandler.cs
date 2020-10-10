using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenusHandler : MonoBehaviour
{
    [SerializeField] private MenuInitialization menuInit = null;

    [Header("Panels")]
    [SerializeField] private List<GameObject> objectsToDisableOnTextOpen = null;

    [field: Header("Panels")]
    [field: SerializeField] public GameObject BattleMenus { get; private set; }
    [field: SerializeField] public GameObject MagicPanel { get; private set; }
    [field: SerializeField] public GameObject TextPanel { get; private set; }
    [field: SerializeField] public GameObject ItemChoicePanel { get; private set; }
    [field: SerializeField] public GameObject Entities { get; private set; }

    [field: Header("Text Area")]
    [field: SerializeField] public TextMeshProUGUI TextArea { get; private set; }

    [field: Header("Battle Commands")]
    [field: SerializeField] public List<BattleCommands> FightMenuCommands { get; private set; }

    [field: Header("Pointer Locations")]
    [field: SerializeField] public Directions[] EnemyChoicePointerLocations { get; private set; } = new Directions[ConstantNumbers.MAX_NUMBER_OF_ENEMIES];
    [field: SerializeField] public Directions[] MagicChoicePointerLocations { get; private set; } = new Directions[ConstantNumbers.MAX_MAGIC_X_LENGTH * ConstantNumbers.MAX_MAGIC_Y_LENGTH];
    [field: SerializeField] public Directions[] ActivePlayerPointerLocation { get; private set; } = new Directions[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];

    [field: Header("Panel Texts")]
    [field: SerializeField] public TextMeshProUGUI[] EnemyIdText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_NUMBER_OF_ENEMIES];
    [field: SerializeField] public TextMeshProUGUI[] PlayerHealthText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];
    [field: SerializeField] public TextMeshProUGUI[] PlayerNameText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];
    [field: SerializeField] public TextMeshProUGUI[] PlayerManaText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS];
    [field: SerializeField] public TextMeshProUGUI[] MagicText { get; private set; } = new TextMeshProUGUI[ConstantNumbers.MAX_MAGIC_X_LENGTH * ConstantNumbers.MAX_MAGIC_Y_LENGTH];

    [field: Header("Pointer")]
    [field: SerializeField] public Image MenuPointer { get; set; }
    public GameObject[] ItemTextBoxes { get; private set; }
    public List<Directions> ItemUsePointerLocations { get; private set; }

    public VectorMenuTraversal VectorMenuNoNulls { get; private set; }
    public BattleEntitySpritePositions BattleEntitySpritePositions { get; private set; }

    

    private void Awake()
    {
        BattleEntitySpritePositions = GetComponent<BattleEntitySpritePositions>();
        if (ConstantNumbers.MAX_NUMBER_OF_ENEMIES < EnemyChoicePointerLocations.Length)
            Debug.LogError("The number of enemyChoice pointer locations exceeds the limit");
        if(ConstantNumbers.MAX_NUMBER_OF_ENEMIES < EnemyIdText.Length)
        {
            Debug.LogError("The number of enemy id textboxes exceeds the limit");
        }
        if (ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS < PlayerHealthText.Length)
            Debug.LogError("the number of player health texts exceeds the limit");
        if (PlayerManaText.Length > ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS)
            Debug.LogError("The max number of mana texts exceeds the limit");
        if(ConstantNumbers.MAX_NUMBER_OF_FIELD_CHARACTERS < PlayerNameText.Length)
        {
            Debug.LogError("The number of player texts exceeds the limit");
        }
        if (ConstantNumbers.MAX_MAGIC_X_LENGTH * ConstantNumbers.MAX_MAGIC_Y_LENGTH < MagicChoicePointerLocations.Length)
            Debug.LogError("The number of magic pointer locations exceeds the limit");
        if (ConstantNumbers.MAX_MAGIC_X_LENGTH * ConstantNumbers.MAX_MAGIC_Y_LENGTH < MagicText.Length)
            Debug.LogError("The number of magic texts exceeds the limit");

        ItemUsePointerLocations = menuInit.InitializePointerLocations();
        ItemTextBoxes = menuInit.InitializeMenuTexts();
    }

    public void PositionPointer(float top, float bottom, float left, float right)
    {
        RectTransformExtensions.SetTop(MenuPointer.rectTransform, top);
        RectTransformExtensions.SetBottom(MenuPointer.rectTransform, bottom);
        RectTransformExtensions.SetLeft(MenuPointer.rectTransform, left);
        RectTransformExtensions.SetRight(MenuPointer.rectTransform, right);
    }

    public void EmptyItemTextBoxes()
    {
        for (int i = 0; i < ItemTextBoxes.Length; i++)
        {
            ItemTextBoxes[i].GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void ClosePanels()
    {
        foreach (GameObject g in objectsToDisableOnTextOpen)
        {
            g.SetActive(false);
        }
    }

    public void OpenTextPanel()
    {
        foreach(GameObject g in objectsToDisableOnTextOpen)
        {
            g.SetActive(false);
        }
        TextPanel.SetActive(true);
    }

    public void CloseTextPanel()
    {
        TextPanel.SetActive(false);
        foreach(GameObject g in objectsToDisableOnTextOpen)
        {
            g.SetActive(true);
        }
    }
}