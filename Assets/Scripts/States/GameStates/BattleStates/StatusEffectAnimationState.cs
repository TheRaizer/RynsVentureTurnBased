using System.Collections.Generic;
using System;
using UnityEngine;

public class StatusEffectAnimationState : State
{
    private readonly BattleHandler battleHandler;
    private readonly BattleTextBoxHandler textHandler;

    public Enum StateToReturnToo { get; set; }
    public List<StatusEffect> StatusEffectsToAnimate { get; set; } = new List<StatusEffect>();
    public StatusEffect ReplacementEffect { get; set; }
    private int index = 0;
    public StatusEffectAnimationState(StateMachine _stateMachine, BattleHandler _battleHandler, BattleTextBoxHandler _textHandler) : base(_stateMachine)
    {
        battleHandler = _battleHandler;
        textHandler = _textHandler;
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
        Debug.Log("Check if finished");
        battleHandler.AnimationsHandler.CheckIfAnimationFinished();
        
    }

    private void RunAnimation()
    {
        battleHandler.CheckForAttackablePlayers();
        battleHandler.CheckForEnemiesRemaining();
        battleHandler.TextMods.ChangeEnemyNameColour();
        battleHandler.TextMods.ChangePlayerTextColour();

        if (CheckIfAnimatedAllEffects()) return;

        battleHandler.AnimationsHandler.OnAnimationFinished = OnEachAnimationFinished;
        StatusEffect currentEffect = StatusEffectsToAnimate[index];
        Debug.Log(currentEffect.Name);
        battleHandler.AnimationsHandler.RunAnim(currentEffect.AnimatedVer.GetComponent<Animator>(), currentEffect.AnimToPlay, currentEffect.TriggerName);
        index++;
    }

    private void OnEachAnimationFinished()
    {
        Debug.Log("Finished");
        if (CheckIfAnimatedAllEffects()) return;
        textHandler.PreviousState = BattleStates.StatusEffectAnimations;
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
        battleHandler.AnimationsHandler.OnAnimationFinished = OnReplacementAnimationFinished;
        if (ReplacementEffect != null)
        {
            battleHandler.AnimationsHandler.RunAnim(ReplacementEffect.AnimatedVer.GetComponent<Animator>(), ReplacementEffect.AnimToPlay, ReplacementEffect.TriggerName);
            ReplacementEffect = null;
        }
        else
        {
            Debug.LogError("supposed to run replacement effect animation, but there is no replacement effect to animate");
        }
    }
    private void OnReplacementAnimationFinished()
    {
        battleHandler.CheckForAttackablePlayers();
        battleHandler.CheckForEnemiesRemaining();
        battleHandler.TextMods.ChangeEnemyNameColour();
        battleHandler.TextMods.ChangePlayerTextColour();
        stateMachine.ChangeState(BattleStates.BattleTextBox);
    }
}
