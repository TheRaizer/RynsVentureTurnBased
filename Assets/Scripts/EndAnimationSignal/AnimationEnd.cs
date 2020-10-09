using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnd : MonoBehaviour
{
    [field: SerializeField] public bool AnimationEnded { get; set; }

    private void SetAnimationEnded()
    {
        AnimationEnded = true;
    }
}
