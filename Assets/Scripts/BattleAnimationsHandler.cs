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
        RanAnim = true;
    }

    public void OnLateUpdate()
    {
        if (RanAnim)
        {
            if (!IsRunningAnim())
            {
                RanAnim = false;
                OnAnimationFinished?.Invoke();
            }
        }
    }

    private bool IsRunningAnim()
    {
        if (!currentAnimator.GetCurrentAnimatorStateInfo(0).IsName(currentAnimClip.name))
        {
            return false;
        }
        else
            return true;
    }
}
