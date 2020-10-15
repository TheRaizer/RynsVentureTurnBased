using System.Collections.Generic;
using System;
using UnityEngine;

public class StatusEffectAnimationState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleEntitiesManager battleEntitiesManager;
    private readonly BattleAnimationsHandler animationsHandler;

    public Enum StateToReturnToo { private get; set; }
    public List<StatusEffect> StatusEffectsToAnimate { get; set; } = new List<StatusEffect>();
    public StatusEffect ReplacementEffect { get; set; }
    private int index = 0;
    public bool CannotAnimateEffects => index == StatusEffectsToAnimate.Count;

    public StatusEffectAnimationState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleTextBoxHandler _textHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        battleEntitiesManager = battleHandler.BattleEntitiesManager;
        animationsHandler = battleHandler.AnimationsHandler;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        Debug.Log("Run Anim");
        Debug.Log("index" + index);
        Debug.Log("Status effect list cout: " + StatusEffectsToAnimate.Count);
        RunAnimation();
    }

    public override void InputUpdate()
    {
        base.InputUpdate();
        animationsHandler.CheckIfAnimationFinished();
        
    }

    public override void OnExit()
    {
        base.OnExit();

        Debug.Log("Exited animation state");
    }

    private void RunAnimation()
    {
        battleEntitiesManager.CheckForAttackablePlayers();
        battleEntitiesManager.CheckForEnemiesRemaining();
        battleHandler.TextMods.ChangeEnemyNameColour();
        battleHandler.TextMods.ChangePlayerTextColour();

        if (CheckIfAnimatedAllEffects()) return;

        animationsHandler.OnAnimationFinished = OnEachAnimationFinished;
        StatusEffect currentEffect = StatusEffectsToAnimate[index];
        Debug.Log(currentEffect.Name);
        animationsHandler.RunAnim(currentEffect.AnimatedVer.GetComponent<Animator>(), currentEffect.AnimToPlay, currentEffect.TriggerName);
        index++;
    }

    private void OnEachAnimationFinished()
    {
        Debug.Log("Finished");
        if (CheckIfAnimatedAllEffects()) return;
        stateMachine.ReturnBackToState(BattleStates.StatusEffectAnimations);
    }

    private bool CheckIfAnimatedAllEffects()
    {
        if (index >= StatusEffectsToAnimate.Count)
        {
            index = 0;
            StatusEffectsToAnimate.Clear();
            StatusEffectsToAnimate.TrimExcess();
            if (StateToReturnToo != null)
            {
                Debug.Log("Return back to " + StateToReturnToo);
                stateMachine.ReturnBackToState(StateToReturnToo);
                StateToReturnToo = null;
            }
            else
            {
                Debug.Log("move to text box");
                stateMachine.ChangeState(BattleStates.BattleTextBox);
            }
            return true;
        }
        return false;
    }

    public void RunReplacementAnimation()
    {
        animationsHandler.OnAnimationFinished = OnReplacementAnimationFinished;
        if (ReplacementEffect != null)
        {
            animationsHandler.RunAnim(ReplacementEffect.AnimatedVer.GetComponent<Animator>(), ReplacementEffect.AnimToPlay, ReplacementEffect.TriggerName);
            ReplacementEffect = null;
        }
        else
        {
            Debug.LogError("supposed to run replacement effect animation, but there is no replacement effect to animate");
        }
    }
    private void OnReplacementAnimationFinished()
    {
        battleEntitiesManager.CheckForAttackablePlayers();
        battleEntitiesManager.CheckForEnemiesRemaining();
        battleHandler.TextMods.ChangeEnemyNameColour();
        battleHandler.TextMods.ChangePlayerTextColour();
        stateMachine.ChangeState(BattleStates.BattleTextBox);
    }
}
