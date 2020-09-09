using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsManager
{
    private readonly TextBoxHandler textBoxHandler;
    private readonly BattleLogic battleLogic;

    public StatusEffectsManager(TextBoxHandler _textBoxHandler, BattleLogic _battleLogic)
    {
        textBoxHandler = _textBoxHandler;
        battleLogic = _battleLogic;
    }

    public bool CheckForStatusEffect(List<StatsManager> attackablePlayers, List<StatsManager> attackableEnemies, StatsManager currentUser)
    {
        bool addedText = false;
        for(int i = 0; i < attackablePlayers.Count; i++)
        {
            for(int j = 0; j < attackablePlayers[i].MultiTurnTriggeredAilments.Count; j++)
            {
                StatusEffect currentStatusAilment = attackableEnemies[i].MultiTurnTriggeredAilments[j].GetComponent<StatusEffect>();

                if (attackablePlayers[i].MultiTurnTriggeredAilments[j].GetComponent<StatusEffect>().HasEnded())
                {
                    textBoxHandler.AddTextAsStatusEffectWornOff(attackablePlayers[i].user.Id, currentStatusAilment.AilmentType);
                    GameObject g = attackablePlayers[i].MultiTurnTriggeredAilments[j];
                    attackablePlayers[i].MultiTurnTriggeredAilments.RemoveAt(j);
                    Object.Destroy(g);
                    addedText = true;
                }
                else
                {
                    currentStatusAilment.OnTurn(attackablePlayers, attackableEnemies, attackablePlayers[i], battleLogic);
                    textBoxHandler.AddTextAsStatusEffect(attackablePlayers[i].user.Id, currentStatusAilment.AilmentType);
                    addedText = true;
                }
            }
        }

        for (int i = 0; i < attackableEnemies.Count; i++)
        {
            for (int j = 0; j < attackableEnemies[i].MultiTurnTriggeredAilments.Count; j++)
            {
                StatusEffect currentStatusAilment = attackableEnemies[i].MultiTurnTriggeredAilments[j].GetComponent<StatusEffect>();
                if (attackableEnemies[i].MultiTurnTriggeredAilments[j].GetComponent<StatusEffect>().HasEnded())
                {
                    textBoxHandler.AddTextAsStatusEffectWornOff(attackableEnemies[i].user.Id, currentStatusAilment.AilmentType);
                    GameObject g = attackableEnemies[i].MultiTurnTriggeredAilments[j];
                    attackableEnemies[i].MultiTurnTriggeredAilments.RemoveAt(j);
                    Object.Destroy(g);
                    addedText = true;
                }
                else
                {
                    currentStatusAilment.OnTurn(attackableEnemies, attackablePlayers, attackableEnemies[i], battleLogic);
                    textBoxHandler.AddTextAsStatusEffect(attackableEnemies[i].user.Id, currentStatusAilment.AilmentType);
                    addedText = true;
                }
            }
        }

        for(int i = 0; i < currentUser.SingleTurnTriggeredAilments.Count; i++)
        {
            StatusEffect currentStatusAilment = currentUser.SingleTurnTriggeredAilments[i].GetComponent<StatusEffect>();

            if (currentStatusAilment.HasEnded())
            {
                currentStatusAilment.OnWornOff(currentUser);
                textBoxHandler.AddTextAsStatusEffectWornOff(currentUser.user.Id, currentStatusAilment.AilmentType);
                GameObject g = currentUser.SingleTurnTriggeredAilments[i];
                currentUser.SingleTurnTriggeredAilments.RemoveAt(i);
                Object.Destroy(g);
                addedText = true;
            }
            else
            {
                currentStatusAilment.OnTurn(attackableEnemies, attackablePlayers, currentUser, battleLogic);
                textBoxHandler.AddTextAsStatusEffect(currentUser.user.Id, currentStatusAilment.AilmentType);
                addedText = true;
            }
        }
        return addedText;
    }

    public bool CheckForReplacementStatusEffect(List<StatsManager> attackablePlayers, List<StatsManager> attackableEnemies, StatsManager currentUser)
    {
        if (currentUser.TurnReplaceAilment != null)
        {
            if(currentUser.TurnReplaceAilment.GetComponent<StatusEffect>().HasEnded())
            {
                GameObject g = currentUser.TurnReplaceAilment;
                textBoxHandler.AddTextAsStatusEffectWornOff(currentUser.user.Id, currentUser.TurnReplaceAilment.GetComponent<StatusEffect>().AilmentType);
                currentUser.TurnReplaceAilment = null;
                Object.Destroy(g);
                return false;
            }
            textBoxHandler.AddTextAsStatusEffect(currentUser.user.Id, currentUser.TurnReplaceAilment.GetComponent<StatusEffect>().AilmentType);
            currentUser.TurnReplaceAilment.GetComponent<StatusEffect>().OnTurn(attackablePlayers, attackableEnemies, currentUser, battleLogic);
            return true;
        }
        else
        {
            return false;
        }
    }
}
