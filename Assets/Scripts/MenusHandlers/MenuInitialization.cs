using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuInitialization
{
    [SerializeField] private int spaceBetweenTexts = 19;
    [SerializeField] private Directions startingPointerLocation = null;
    [SerializeField] private int numOfTextBoxes = 40;

    [SerializeField] private RectTransform textPrefab;
    [SerializeField] private GameObject conentPanel;
    [SerializeField] private Directions textBoxStartingDirection;

    public List<Directions> InitializePointerLocations()
    {
        List<Directions> pointerLocations = new List<Directions>();
        Directions start = new Directions
        {
            top = startingPointerLocation.top,
            bottom = startingPointerLocation.bottom,
            left = startingPointerLocation.left,
            right = startingPointerLocation.right
        };

        pointerLocations.Add(start);

        for (int i = 1; i < numOfTextBoxes; i++)
        {
            startingPointerLocation.top += spaceBetweenTexts;
            startingPointerLocation.bottom -= spaceBetweenTexts;

            Directions dir = new Directions
            {
                top = startingPointerLocation.top,
                bottom = startingPointerLocation.bottom,
                right = startingPointerLocation.right,
                left = startingPointerLocation.left
            };

            pointerLocations.Add(dir);
        }

        return pointerLocations;
    }

    public GameObject[] InitializeMenuTexts()
    {
        GameObject[] textArr;
        List<GameObject> texts = new List<GameObject>();
        Directions refDir = textBoxStartingDirection;

        GameObject firstTextBox = Object.Instantiate(textPrefab.gameObject);
        firstTextBox.transform.SetParent(conentPanel.transform);

        RectTransform r = firstTextBox.GetComponent<RectTransform>();

        RectTransformExtensions.SetTop(r, refDir.top);
        RectTransformExtensions.SetBottom(r, refDir.bottom);
        RectTransformExtensions.SetLeft(r, refDir.left);
        RectTransformExtensions.SetRight(r, refDir.right);

        texts.Add(firstTextBox);

        for (int i = 1; i < numOfTextBoxes; i++)
        {
            GameObject currentTextBox = Object.Instantiate(textPrefab.gameObject);
            currentTextBox.transform.SetParent(conentPanel.transform);

            RectTransform currentTextBoxRect = currentTextBox.GetComponent<RectTransform>();
            refDir.top += spaceBetweenTexts;
            refDir.bottom -= spaceBetweenTexts;

            RectTransformExtensions.SetTop(currentTextBoxRect, refDir.top);
            RectTransformExtensions.SetBottom(currentTextBoxRect, refDir.bottom);
            RectTransformExtensions.SetLeft(currentTextBoxRect, refDir.left);
            RectTransformExtensions.SetRight(currentTextBoxRect, refDir.right);

            texts.Add(currentTextBox);
        }

        textArr = texts.ToArray();

        return textArr;
    }
}
