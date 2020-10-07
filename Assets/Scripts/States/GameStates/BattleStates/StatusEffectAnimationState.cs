using System.Collections.Generic;
using System;
using UnityEngine;

public class StatusEffectAnimationState : State
{
    private readonly BattleLogic battleLogic;
    private readonly BattleTextBoxHandler textHandler;

    public Enum stateToReturnToo;
    public List<StatusEffect> StatusEffectsToAnimate { get; set; } = new List<StatusEffect>();
    public StatusEffect ReplacementEffect { get; set; }
    private int index = 0;

    public StatusEffectAnimationState(StateMachine _stateMachine, BattleLogic _battleLogic, BattleTextBoxHandler _textHandler) : base(_stateMachine)
    {
        battleLogic = _battleLogic;
        textHandler = _textHandler;
    }

    public override void OnEnterOrReturn()
    {
        base.OnEnterOrReturn();

        Debug.Log(StatusEffectsToAnimate.Count);
        Debug.Log("Index " + index);

        battleLogic.AnimationsHandler.OnAnimationFinished = OnEachAnimationFinished;
        StatusEffect currentEffect = StatusEffectsToAnimate[index];
        battleLogic.AnimationsHandler.RunAnim(currentEffect.AnimatedVer.GetComponent<Animator>(), currentEffect.AnimToPlay, currentEffect.TriggerName);
        index++;
        

        if(ReplacementEffect != null)
        {
            battleLogic.AnimationsHandler.RunAnim(ReplacementEffect.AnimatedVer.GetComponent<Animator>(), ReplacementEffect.AnimToPlay, ReplacementEffect.TriggerName);
            ReplacementEffect = null;
        }
    }

    public override void InputUpdate()
    {
        base.InputUpdate();

        battleLogic.AnimationsHandler.OnLateUpdate();
    }

    private void OnEachAnimationFinished()
    {
        Debug.Log("Finished");
        battleLogic.CheckForAttackablePlayers();
        battleLogic.CheckForEnemiesRemaining();
        battleLogic.TextMods.ChangeEnemyNameColour();
        battleLogic.TextMods.ChangePlayerTextColour();
        if(index >= StatusEffectsToAnimate.Count)
        {
            index = 0;
            StatusEffectsToAnimate.Clear();
            StatusEffectsToAnimate.TrimExcess();
            stateMachine.ChangeState(BattleStates.BattleTextBox);
            return;
        }
        textHandler.PreviousState = BattleStates.StatusEffectAnimations;
        stateMachine.ReturnBackToState(BattleStates.StatusEffectAnimations);
    }
}
