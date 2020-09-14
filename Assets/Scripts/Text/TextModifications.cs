using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextModifications
{
    private const float PERCENT_TILL_YELLOW = 0.5f;
    private const float PERCENT_TILL_RED = 0.25f;

    

    private readonly MenusHandler menusHandler;
    private readonly BattleLogic battleLogic;

    public TextModifications(MenusHandler _menusHandler, BattleLogic _battleLogic)
    {
        menusHandler = _menusHandler;
        battleLogic = _battleLogic;
    }

    public void PrintEnemyIds()
    {
        for (int i = 0; i < battleLogic.Enemies.Length; i++)
        {
            if (battleLogic.Enemies[i] != null)
            {
                menusHandler.EnemyIdText[i].text = battleLogic.Enemies[i].GetComponent<Enemy>().Id;
            }
        }
    }

    public void PrintPlayerIds()
    {
        for (int i = 0; i < battleLogic.ActivePlayableCharacters.Length; i++)
        {
            if (battleLogic.ActivePlayableCharacters[i] != null)
            {
                menusHandler.PlayerNameText[i].text = battleLogic.ActivePlayableCharacters[i].Id;
            }
        }
    }

    public void PrintPlayerHealth()
    {
        for (int i = 0; i < battleLogic.ActivePlayableCharacters.Length; i++)
        {
            if (battleLogic.ActivePlayableCharacters[i] != null)
            {
                menusHandler.PlayerHealthText[i].text = battleLogic.ActivePlayableCharacters[i].Stats.HealthManager.CurrentAmount + " / " + battleLogic.ActivePlayableCharacters[i].Stats.HealthManager.MaxAmount;
            }
        }
    }

    public void ChangeEnemyNameColour()
    {
        for (int i = 0; i < battleLogic.Enemies.Length; i++)
        {
            if (battleLogic.Enemies[i] != null)
            {
                StatsManager currentEnemy = battleLogic.Enemies[i].GetComponent<StatsManager>();

                float yellowAmt = currentEnemy.HealthManager.MaxAmount * PERCENT_TILL_YELLOW;
                float redAmt = currentEnemy.HealthManager.MaxAmount * PERCENT_TILL_RED;

                if (currentEnemy.HealthManager.CurrentAmount <= redAmt)
                {
                    menusHandler.EnemyIdText[i].color = Color.red;
                }
                else if (currentEnemy.HealthManager.CurrentAmount <= yellowAmt)
                {
                    menusHandler.EnemyIdText[i].color = Color.yellow;
                }
            }
        }
    }

    public void ChangePlayerTextColour()
    {
        for (int i = 0; i < battleLogic.ActivePlayableCharacters.Length; i++)
        {
            if (battleLogic.ActivePlayableCharacters[i] != null)
            {
                if (!battleLogic.ActivePlayableCharacters[i].Stats.HealthManager.Dead)
                {
                    StatsManager currentPlayer = battleLogic.ActivePlayableCharacters[i].Stats;

                    float yellowAmt = currentPlayer.HealthManager.MaxAmount * PERCENT_TILL_YELLOW;
                    float redAmt = currentPlayer.HealthManager.MaxAmount * PERCENT_TILL_RED;

                    if (currentPlayer.HealthManager.CurrentAmount <= redAmt)
                    {
                        menusHandler.PlayerHealthText[i].color = Color.red;
                        menusHandler.PlayerNameText[i].color = Color.red;
                    }
                    else if (currentPlayer.HealthManager.CurrentAmount <= yellowAmt)
                    {
                        menusHandler.PlayerHealthText[i].color = Color.yellow;
                        menusHandler.PlayerNameText[i].color = Color.yellow;
                    }
                }
            }
        }
    }
}
