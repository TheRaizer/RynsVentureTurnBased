using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    public static T Get1DElementFrom2DArray<T>(this T[] arr, int width, int row, int col)
    {
        int index = width * row + col;
        return arr[index];
    }
}
