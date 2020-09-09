using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DirectionAnim()
    {
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        if(horiz > 0)
        {
            animator.SetInteger("Direction", 3);
        }
        else if(horiz < 0)
        {
            animator.SetInteger("Direction", 2);
        }
        
        if(vert > 0)
        {
            animator.SetInteger("Direction", 0);
        }
        else if(vert < 0)
        {
            animator.SetInteger("Direction", 1);
        }
    }
}
