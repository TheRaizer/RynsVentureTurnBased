﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldMenusHandler : MonoBehaviour
{
    [SerializeField] private MenuInitialization menuInit = null;

    [field: Header("Menu Options")]
    [field: SerializeField] public List<MenuOption> StartingMenuOptions { get; private set; }
    [field: SerializeField] public List<InventoryTypeChoiceOption> InventoryChoiceOptions { get; private set; }
    [field: SerializeField] public GameObject MenuPanel { get; private set; }

    [field: Header("Menu Pointer")]
    [field: SerializeField] public Image Pointer { get; private set; }

    public GameObject[] TextBoxes { get; private set; }
    private VectorMenuTraversal currentMenuTraversal;
    private List<Directions> pointerLocations = new List<Directions>();

    private void Awake()
    {
        pointerLocations = menuInit.InitializePointerLocations();
        TextBoxes = menuInit.InitializeMenuTexts();

        Debug.Log(pointerLocations.Count);
        Debug.Log(TextBoxes.Length);

        currentMenuTraversal = new VectorMenuTraversal(PositionPointer)
        {
            MaxIndex = TextBoxes.Length - 1
        };

    }

    private void Start()
    {
        PositionPointer();
    }

    public void PositionPointer()
    {
        PositionUIObjectEventType positionUIObjectEventType = new PositionUIObjectEventType(pointerLocations[currentMenuTraversal.currentIndex], Pointer.rectTransform);
        PositionUIObjectPublisher publisher = new PositionUIObjectPublisher();
        publisher.Publish(positionUIObjectEventType);
    }

    public void EmptyTextBoxes()
    {
        for(int i = 0; i < TextBoxes.Length; i++)
        {
            TextBoxes[i].GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public int MenuTraversalCurrentIndex => currentMenuTraversal.currentIndex;
    public void SetMenuTraversalCurrentIndex(int index)
    {
        currentMenuTraversal.currentIndex = index;
    }
    public void SetMenuTraversalMaxIndex(int maxIndex) => currentMenuTraversal.MaxIndex = maxIndex;
    public void ResetPointerPosition() 
    {
        currentMenuTraversal.currentIndex = 0;
        PositionPointer();
    }
    public void Traverse() => currentMenuTraversal.Traverse();
}
