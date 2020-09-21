using System;
using UnityEngine;

public class VectorMenuTraversal
{
    public int currentIndex = 0;
    private readonly Action onTraversal;

    public int MaxIndex { get; set; }

    public VectorMenuTraversal(Action _onTraversal)
    {
        onTraversal = _onTraversal;
    }

    public void Traverse()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentIndex--;
            CheckIfIndexInRange();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentIndex++;
            CheckIfIndexInRange();
        }
    }

    public void TraverseWithNulls<T>(T[] menu)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentIndex--;
            CheckIfIndexInRange();
            while (menu[currentIndex] == null)
            {
                currentIndex--;
                CheckIfIndexInRange();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentIndex++;
            CheckIfIndexInRange();

            while (menu[currentIndex] == null)
            {
                currentIndex++;
                CheckIfIndexInRange();
            }
        }
    }

    public void CheckIfIndexInRange()
    {
        if (currentIndex > MaxIndex)
            currentIndex = 0;
        else if (currentIndex < 0)
            currentIndex = MaxIndex;

        onTraversal?.Invoke();
    }

    public void ResetCurrentIndex()
    {
        currentIndex = 0;
    }
}
