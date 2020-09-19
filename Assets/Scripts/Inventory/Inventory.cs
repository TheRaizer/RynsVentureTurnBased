using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject hpPotionPrefab;
    [SerializeField] private GameObject attackUpPrefab;

    [Header("TextBox generation fields")]
    [SerializeField] private RectTransform content = null;
    [SerializeField] private RectTransform itemTextBoxPrefab = null;
    [SerializeField] private int numberOfTextBoxesToGenerate = 10;

    [SerializeField] private Directions referenceDirections = new Directions();
    [SerializeField] private GameObject inventoryPanel = null;

    [Header("Inventory Pointer")]
    [SerializeField] private RectTransform pointer = null;
    [SerializeField] private Directions startingPointerLocation;

    private GameObject[] textBoxes;
    private readonly List<Directions> pointerLocations = new List<Directions>();
    private readonly Dictionary<Type, IList> inventoryDic = new Dictionary<Type, IList>();
    private VectorMenuTraversal inventoryTraversal;

    private const int SPACE_BETWEEN_TEXTS = 19;

    private Type currentInventoryOpen = typeof(Useable);

    private void Awake()
    {
        inventoryDic.Add(typeof(Useable), new List<Useable>());
        inventoryTraversal = new VectorMenuTraversal(PositionPointer)
        {
            MaxIndex = inventoryDic[currentInventoryOpen].Count - 1
        };


        referenceDirections.top = -itemTextBoxPrefab.offsetMax.y;
        referenceDirections.bottom = itemTextBoxPrefab.offsetMin.y;
        referenceDirections.left = itemTextBoxPrefab.offsetMin.x;
        referenceDirections.right = itemTextBoxPrefab.offsetMax.x;

        InitializeWorldInventoryTexts();
        InitializePointerLocations();
        PositionPointer();

        Useable hpPotion_1 = hpPotionPrefab.GetComponent<SmallHpPotion>().ShallowClone();
        Useable hpPotion_2 = hpPotionPrefab.GetComponent<SmallHpPotion>().ShallowClone();
        Useable attackUp = attackUpPrefab.GetComponent<SmallAttackUp>().ShallowClone();
        hpPotion_1.Amount += 15;
        hpPotion_2.Amount = 1;
        attackUp.Amount = 1;
        AddToInventory(attackUp);
        AddToInventory(hpPotion_1);
        AddToInventory(hpPotion_2);
        RemoveFromInventory(1, 1, hpPotion_1.GetType().BaseType);

        PrintUseableInventoryWorldText();
    }

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
        if (inventoryPanel.activeSelf)
        {
            inventoryTraversal.Traverse(textBoxes);
        }
    }


    public void ChangeOpenedInventory(Type inventoryType)
    {
        currentInventoryOpen = inventoryType;
        inventoryTraversal.MaxIndex = inventoryDic[currentInventoryOpen].Count - 1;
    }

    private void PositionPointer()
    {
        RectTransformExtensions.SetTop(pointer, pointerLocations[inventoryTraversal.currentIndex].top);
        RectTransformExtensions.SetBottom(pointer, pointerLocations[inventoryTraversal.currentIndex].bottom);
        RectTransformExtensions.SetLeft(pointer, pointerLocations[inventoryTraversal.currentIndex].left);
        RectTransformExtensions.SetRight(pointer, pointerLocations[inventoryTraversal.currentIndex].right);
    }

    private void InitializePointerLocations()
    {
        Directions start = new Directions
        {
            top = startingPointerLocation.top,
            bottom = startingPointerLocation.bottom,
            left = startingPointerLocation.left,
            right = startingPointerLocation.right
        };

        pointerLocations.Add(start);

        for(int i = 1; i <= numberOfTextBoxesToGenerate; i++)
        {
            startingPointerLocation.top += SPACE_BETWEEN_TEXTS;
            startingPointerLocation.bottom -= SPACE_BETWEEN_TEXTS;

            Directions dir = new Directions
            {
                top = startingPointerLocation.top,
                bottom = startingPointerLocation.bottom,
                right = startingPointerLocation.right,
                left = startingPointerLocation.left
            };

            pointerLocations.Add(dir);
        }
    }

    private void InitializeWorldInventoryTexts()
    {
        List<GameObject> texts = new List<GameObject>();
        GameObject firstTextBox = Instantiate(itemTextBoxPrefab.gameObject);
        firstTextBox.transform.SetParent(content.transform);

        RectTransform r = firstTextBox.GetComponent<RectTransform>();

        RectTransformExtensions.SetTop(r, referenceDirections.top);
        RectTransformExtensions.SetBottom(r, referenceDirections.bottom);
        RectTransformExtensions.SetLeft(r, referenceDirections.left);
        RectTransformExtensions.SetRight(r, referenceDirections.right);

        texts.Add(firstTextBox);

        for(int i = 1; i < numberOfTextBoxesToGenerate; i++)
        {
            GameObject currentTextBox = Instantiate(itemTextBoxPrefab.gameObject);
            currentTextBox.transform.SetParent(content.transform);

            RectTransform currentTextBoxRect = currentTextBox.GetComponent<RectTransform>();
            referenceDirections.top += SPACE_BETWEEN_TEXTS;
            referenceDirections.bottom -= SPACE_BETWEEN_TEXTS;

            RectTransformExtensions.SetTop(currentTextBoxRect, referenceDirections.top);
            RectTransformExtensions.SetBottom(currentTextBoxRect, referenceDirections.bottom);
            RectTransformExtensions.SetLeft(currentTextBoxRect, referenceDirections.left);
            RectTransformExtensions.SetRight(currentTextBoxRect, referenceDirections.right);

            texts.Add(currentTextBox);
        }

        textBoxes = texts.ToArray();
        inventoryPanel.SetActive(false);
    }

    public void PrintUseableInventoryWorldText()
    {
        for (int i = 0; i < inventoryDic[typeof(Useable)].Count; i++)
        {
            TextMeshProUGUI textMesh = textBoxes[i].GetComponent<TextMeshProUGUI>();

            Item item = (Item)inventoryDic[typeof(Useable)][i];

            textMesh.text = item.Amount + " " + item.Id;
        }
    }

    public void AddToInventory<T>(T itemToAdd)
    {
        IStoreable item = (IStoreable)itemToAdd;
        Type t = itemToAdd.GetType();
        IList inventory = inventoryDic[t.BaseType];

        if (inventory.Contains(itemToAdd))
        {
            int index = inventory.IndexOf(itemToAdd);
            IStoreable storeable = (IStoreable)inventory[index];

            if(storeable.Amount + item.Amount >= storeable.MaxHoldable)
            {
                Debug.Log("Cant carry any more");
            }
            else
                storeable.Amount += item.Amount;
        }
        else
        {
            inventory.Add(item);
            if(currentInventoryOpen == t.BaseType)
            {
                inventoryTraversal.MaxIndex = inventory.Count - 1;
            }
        }
    }

    public void UseItemInventoryInWorld(int indexToRemove, StatsManager entityToUseOn)
    {
        Useable itemToUse = (Useable)inventoryDic[typeof(Useable)][indexToRemove];
        if (inventoryDic[typeof(Useable)].Contains(itemToUse))
        {
            itemToUse.OnUseInWorld(entityToUseOn);

            if(itemToUse.IsEmpty)
            {
                inventoryDic[typeof(Useable)].Remove(itemToUse);
                if(currentInventoryOpen == typeof(Useable))
                {
                    inventoryTraversal.MaxIndex = inventoryDic[typeof(Useable)].Count - 1;
                }
            }
        }
        else
        {
            Debug.Log("NO item in inventory");
        }
    }

    public void RemoveFromInventory(int amount, int indexToRemove, Type inventoryType)
    {
        IList inventoryToRemoveFrom = inventoryDic[inventoryType];

        IStoreable item = (IStoreable)inventoryToRemoveFrom[indexToRemove];
        if (item.Amount - amount <= 0)
        {
            inventoryToRemoveFrom.Remove(inventoryToRemoveFrom[indexToRemove]);
            if(currentInventoryOpen == inventoryType)
            {
                inventoryTraversal.MaxIndex = inventoryToRemoveFrom.Count - 1;
            }
        }
        else
        {
            item.Amount -= amount;
        }
    }
}

public interface IStoreable
{
    int Amount { get; set; }
    int MaxHoldable { get; }
}
