using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationsHandler
{
    private readonly BattleMenusHandler menusHandler;
    public bool RanAnim { get; private set; }
    private AnimationClip currentAnimClip;
    private Animator currentAnimator;
    private AnimationEnd animationEnd;
    public Action OnAnimationFinished { get; set; }

    public BattleAnimationsHandler(BattleMenusHandler _menusHandler)
    {
        menusHandler = _menusHandler;
    }

    public void RunAnim(Animator animator, AnimationClip animation, string triggerName)
    {
        menusHandler.ClosePanels();
        animator.SetTrigger(triggerName);
        currentAnimator = animator;
        currentAnimClip = animation;
        animationEnd = currentAnimator.gameObject.GetComponent<AnimationEnd>();
        RanAnim = true;
        Debug.Log("Run anim " + currentAnimClip);
    }

    public void CheckIfAnimationFinished()
    {
        if (RanAnim)
        {
            if (animationEnd.AnimationEnded)
            {
                var m_CurrentClipInfo = currentAnimator.GetCurrentAnimatorClipInfo(0);
                Debug.Log("Finished playing " + currentAnimClip);
                Debug.Log("Entered " + m_CurrentClipInfo[0].clip.name);
                RanAnim = false;
                animationEnd.AnimationEnded = false;
                OnAnimationFinished?.Invoke();
            }
        }
    }
}
