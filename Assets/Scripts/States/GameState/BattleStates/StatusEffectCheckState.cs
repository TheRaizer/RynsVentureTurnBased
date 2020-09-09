public abstract class StatusEffectCheckState : State
{
    public abstract bool CheckedStatusEffectThisTurn { get; set; }
    public StatusEffectCheckState(StateMachine _stateMachine) : base(_stateMachine)
    {

    }

    protected bool CheckForStatusEffects(StatusEffectsManager ailmentsManager, BattleLogic battleLogic, TextBoxHandler textBoxHandler, StatsManager user, State currentState)
    {
        if (!CheckedStatusEffectThisTurn)
        {
            CheckedStatusEffectThisTurn = true;
            if (ailmentsManager.CheckForStatusEffect(battleLogic.attackablePlayers, battleLogic.AttackableEnemies, user))
            {
                if (!user.HealthManager.Dead)
                {
                    textBoxHandler.previousState = currentState;
                }
                return true;
            }
            
        }
        CheckedStatusEffectThisTurn = false;

        return false;
    }
}
