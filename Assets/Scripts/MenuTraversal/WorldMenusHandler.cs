using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldMenusHandler : MonoBehaviour
{
    [field: Header("Menu Options")]
    [field: SerializeField] public List<MenuOption> StartingMenuOptions { get; private set; }
    [field: SerializeField] public List<InventoryTypeChoiceOption> InventoryChoiceOptions { get; private set; }
     
    [Header("TextBox generation fields")]
    [SerializeField] private RectTransform content = null;
    [SerializeField] private RectTransform textBoxPrefab = null;
    [SerializeField] private int maxNumberOfTextBoxesToGenerate = 40;
    [field: SerializeField] public GameObject MenuPanel { get; private set; }

    [field: Header("Menu Pointer")]
    [field: SerializeField] public Image Pointer { get; private set; }
    [SerializeField] private Directions startingPointerLocation = null;

    public GameObject[] TextBoxes { get; private set; }
    private VectorMenuTraversal currentMenuTraversal;
    private readonly List<Directions> pointerLocations = new List<Directions>();
    private readonly Directions referenceDirections = new Directions();

    private const int SPACE_BETWEEN_TEXTS = 19;

    private void Awake()
    {
        referenceDirections.top = -textBoxPrefab.offsetMax.y;
        referenceDirections.bottom = textBoxPrefab.offsetMin.y;
        referenceDirections.left = textBoxPrefab.offsetMin.x;
        referenceDirections.right = textBoxPrefab.offsetMax.x;

        InitializePointerLocations();
        InitializeWorldMenuTexts();

        currentMenuTraversal = new VectorMenuTraversal(PositionPointer)
        {
            MaxIndex = TextBoxes.Length - 1
        };

        PositionPointer();
    }

    private void PositionPointer()
    {
        RectTransformExtensions.SetBottom(Pointer.rectTransform, pointerLocations[currentMenuTraversal.currentIndex].bottom);
        RectTransformExtensions.SetLeft(Pointer.rectTransform, pointerLocations[currentMenuTraversal.currentIndex].left);
        RectTransformExtensions.SetRight(Pointer.rectTransform, pointerLocations[currentMenuTraversal.currentIndex].right);
        RectTransformExtensions.SetTop(Pointer.rectTransform, pointerLocations[currentMenuTraversal.currentIndex].top);
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

        for (int i = 1; i <= maxNumberOfTextBoxesToGenerate; i++)
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

    private void InitializeWorldMenuTexts()
    {
        List<GameObject> texts = new List<GameObject>();
        GameObject firstTextBox = Instantiate(textBoxPrefab.gameObject);
        firstTextBox.transform.SetParent(content.transform);

        RectTransform r = firstTextBox.GetComponent<RectTransform>();

        RectTransformExtensions.SetTop(r, referenceDirections.top);
        RectTransformExtensions.SetBottom(r, referenceDirections.bottom);
        RectTransformExtensions.SetLeft(r, referenceDirections.left);
        RectTransformExtensions.SetRight(r, referenceDirections.right);

        texts.Add(firstTextBox);

        for (int i = 1; i < maxNumberOfTextBoxesToGenerate; i++)
        {
            GameObject currentTextBox = Instantiate(textBoxPrefab.gameObject);
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

        TextBoxes = texts.ToArray();
        MenuPanel.SetActive(false);
    }

    public void EmptyTextBoxes()
    {
        for(int i = 0; i < TextBoxes.Length; i++)
        {
            TextBoxes[i].GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public int MenuTraversalCurrentIndex => currentMenuTraversal.currentIndex;
    public void SetMenuTraversalMaxIndex(int maxIndex) => currentMenuTraversal.MaxIndex = maxIndex;
    public void ResetPointerPosition() 
    {
        currentMenuTraversal.currentIndex = 0;
        PositionPointer();
    }
    public void Traverse() => currentMenuTraversal.Traverse();
}
