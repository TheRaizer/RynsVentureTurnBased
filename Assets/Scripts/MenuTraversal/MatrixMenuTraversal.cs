using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixMenuTraversal
{
    public int currentXIndex = 0;
    public int currentYIndex = 0;

    public int MaxXIndex { get; set; }
    public int MaxYIndex { get; set; }

    private readonly Action onTraversal;

    public MatrixMenuTraversal(Action _onTraversal)
    {
        onTraversal = _onTraversal;
    }

    public void TraverseWithNulls<T>(T[,] menu)
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentXIndex++;
            CheckIfIndexInRange();
            while(menu[currentXIndex, currentYIndex] == null)
            {
                currentXIndex++;
                CheckIfIndexInRange();
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentXIndex--;
            CheckIfIndexInRange();
            while(menu[currentXIndex, currentYIndex] == null)
            {
                currentXIndex--;
                CheckIfIndexInRange();
            }
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentYIndex--;
            CheckIfIndexInRange();
            while(menu[currentXIndex, currentYIndex] == null)
            {
                currentYIndex--;
                CheckIfIndexInRange();
            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentYIndex++;
            CheckIfIndexInRange();
            while(menu[currentXIndex, currentYIndex] == null)
            {
                currentYIndex++;
                CheckIfIndexInRange();
            }
        }
    }

    public void Traverse()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentXIndex++;
            CheckIfIndexInRange();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentXIndex--;
            CheckIfIndexInRange();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            currentYIndex--;
            CheckIfIndexInRange();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            currentYIndex++;
            CheckIfIndexInRange();
        }
    }

    public void CheckIfIndexInRange()
    {
        if (currentXIndex > MaxXIndex)
            currentXIndex = 0;
        else if (currentXIndex < 0)
            currentXIndex = MaxXIndex;

        if (currentYIndex > MaxYIndex)
            currentYIndex = 0;
        else if (currentYIndex < 0)
            currentYIndex = MaxYIndex;

        onTraversal?.Invoke();
    }

    public void ResetIndexes()
    {
        currentXIndex = 0;
        currentYIndex = 0;
    }
}
