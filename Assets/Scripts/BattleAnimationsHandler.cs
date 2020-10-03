using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnimationsHandler
{
    private readonly BattleMenusHandler menusHandler;
    public bool RanAnim { get; set; }
    public AnimationClip CurrentAnimClip { get; set; }
    public Animator CurrentAnimator { get; set; }

    public BattleAnimationsHandler(BattleMenusHandler _menusHandler)
    {
        menusHandler = _menusHandler;
    }

    public void RunAnim(Animator animator, AnimationClip animation, string triggerName)
    {
        menusHandler.ClosePanels();
        animator.SetTrigger(triggerName);
        CurrentAnimator = animator;
        CurrentAnimClip = animation;
        RanAnim = true;
    }

    public bool IsRunningAnim()
    {
        if (!CurrentAnimator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnimClip.name))
        {
            return false;
        }
        else
            return true;
    }
}
