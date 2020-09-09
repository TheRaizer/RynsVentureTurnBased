using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TextManipulation
{
    public static string[] ConvertTextAssetToStringArray(this TextAsset text)
    {
        return text.text.Split('\n');
    }
}
