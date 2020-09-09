using System;
using UnityEngine;

public class VectorMenuTraversal
{
    public int currentIndex = 0;
    private readonly Action onTraversal;

    public int Min { get; set; }
    public int Max { get; set; }

    public VectorMenuTraversal(Action _onTraversal)
    {
        onTraversal = _onTraversal;
    }

    public void Traverse<T>(T[] arr)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            CheckIfIndexInRange();
            while (arr[currentIndex] == null)
            {
                currentIndex--;
                CheckIfIndexInRange();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            CheckIfIndexInRange();

            while (arr[currentIndex] == null)
            {
                currentIndex++;
                CheckIfIndexInRange();
            }
        }
    }

    public void CheckIfIndexInRange()
    {
        if (currentIndex >= Max)
            currentIndex = 0;
        else if (currentIndex < Min)
            currentIndex = Max - 1;

        onTraversal?.Invoke();
    }

    public void ResetCurrentIndex()
    {
        currentIndex = 0;
    }
}
