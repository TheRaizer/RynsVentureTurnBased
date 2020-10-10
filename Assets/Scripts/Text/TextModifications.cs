using UnityEngine;

public class TextModifications
{
    private const float PERCENT_TILL_YELLOW = 0.5f;
    private const float PERCENT_TILL_RED = 0.25f;

    private readonly BattleMenusHandler menusHandler;
    private readonly BattleEntitiesManager battleEntitiesManager;

    public TextModifications(BattleMenusHandler _menusHandler, BattleHandler _battleHandler)
    {
        menusHandler = _menusHandler;
        battleEntitiesManager = _battleHandler.BattleEntitiesManager;
    }

    public void PrintEnemyIds()
    {
        for (int i = 0; i < battleEntitiesManager.Enemies.Length; i++)
        {
            if (battleEntitiesManager.Enemies[i] != null)
            {
                menusHandler.EnemyIdText[i].text = battleEntitiesManager.Enemies[i].GetComponent<Enemy>().Id;
            }
        }
    }

    public void PrintPlayerIds()
    {
        for (int i = 0; i < battleEntitiesManager.ActivePlayableCharacters.Length; i++)
        {
            if (battleEntitiesManager.ActivePlayableCharacters[i] != null)
            {
                menusHandler.PlayerNameText[i].text = battleEntitiesManager.ActivePlayableCharacters[i].Id;
            }
        }
    }

    public void PrintPlayerHealth()
    {
        for (int i = 0; i < battleEntitiesManager.ActivePlayableCharacters.Length; i++)
        {
            if (battleEntitiesManager.ActivePlayableCharacters[i] != null)
            {
                menusHandler.PlayerHealthText[i].text = battleEntitiesManager.ActivePlayableCharacters[i].Stats.HealthManager.CurrentAmount +
                                                        " / " + battleEntitiesManager.ActivePlayableCharacters[i].Stats.HealthManager.MaxAmount;
            }
        }
    }

    public void PrintPlayerMana()
    {
        for (int i = 0; i < battleEntitiesManager.ActivePlayableCharacters.Length; i++)
        {
            if (battleEntitiesManager.ActivePlayableCharacters[i] != null)
            {
                menusHandler.PlayerManaText[i].text = battleEntitiesManager.ActivePlayableCharacters[i].Stats.ManaManager.CurrentAmount.ToString();
            }
        }
    }

    public void ChangeEnemyNameColour()
    {
        for (int i = 0; i < battleEntitiesManager.Enemies.Length; i++)
        {
            if (battleEntitiesManager.Enemies[i] != null)
            {
                StatsManager currentEnemy = battleEntitiesManager.Enemies[i].GetComponent<StatsManager>();

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
        for (int i = 0; i < battleEntitiesManager.ActivePlayableCharacters.Length; i++)
        {
            if (battleEntitiesManager.ActivePlayableCharacters[i] != null)
            {
                if (!battleEntitiesManager.ActivePlayableCharacters[i].Stats.HealthManager.Dead)
                {
                    StatsManager currentPlayer = battleEntitiesManager.ActivePlayableCharacters[i].Stats;

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
