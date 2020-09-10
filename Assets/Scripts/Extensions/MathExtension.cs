using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtension
{
    public static int RoundToNearestInteger(this float value)
    {
        if(value % 0.5f == 0)
        {
            return (int)Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }
        else
            return (int)Math.Round(value, 0);
    }
}
