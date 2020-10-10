using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTextBoxHandler
{
    public Enum PreviousState { get; set; } = null;

    private readonly BattleMenusHandler menusHandler;
    private readonly BattleHandler battleLogic;
    private readonly StateMachine battleStateMachine;

    private readonly WaitForSeconds writeTime = new WaitForSeconds(0.08f);
    private int currentLine = 0;
    private bool Finished;
    private bool running;
    private bool skip;
    private readonly List<string> textLines = new List<string>();

    public BattleTextBoxHandler(BattleMenusHandler _menusHandler, BattleHandler _battleLogic, StateMachine _battleStateMachine)
    {
        menusHandler = _menusHandler;
        battleLogic = _battleLogic;
        battleStateMachine = _battleStateMachine;
    }

    public void InputUpdate()
    {
        bool next = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E);

        if (running && next)
        {
            skip = true;
        }
        if (!running && !Finished && next)
        {
            menusHandler.StartCoroutine(BuildMultiStringTextCo());
        }
        if (Finished && next)
        {
            ResetTextBox();
            Finished = false;
            if(PreviousState != null)
            {
                ReturnToPreviousState();
            }
            else
                battleLogic.BattleEntitiesManager.CalculateNextTurn();
        }
    }

    public IEnumerator BuildMultiStringTextCo()
    {
        if (!running)
        {
            running = true;
            Finished = false;
            menusHandler.TextArea.text = "";

            if (currentLine >= textLines.Count)
            {
                ResetTextBox();
                Finished = true;
                yield break;
            }

            for (int i = 0; i < textLines[currentLine].Length; i++)
            {
                menusHandler.TextArea.text = string.Concat(menusHandler.TextArea.text, textLines[currentLine][i]);

                if (skip)
                {
                    skip = false;
                    menusHandler.TextArea.text = textLines[currentLine];
                    break;
                }

                yield return writeTime;
            }

            currentLine++;
            running = false;
        }
    }

    public void ResetTextBox()
    {
        menusHandler.StopCoroutine(BuildMultiStringTextCo());
        running = false;
        skip = false;
        currentLine = 0;
        textLines.Clear();
    }

    public void AddTextLines(string value)
    {
        textLines.Add(value);
    }

    public void AddTextAsTurn(string userId)
    {
        AddTextLines("It is " + userId + "'s Turn.");
    }

    public void AddTextAsStatusInfliction(string userId, string foeId, string statusEffectName)
    {
        AddTextLines(userId + " has inflicted " + statusEffectName + " on " + foeId);
    }
    public void AddTextAsAttack(string userId, string attackText, string foeId)
    {
        AddTextLines(userId + " " + attackText + " " + foeId + ".");
    }

    public void AddTextOnMiss(string userId, string foeId)
    {
        AddTextLines(userId + " missed " + foeId + ".");
    }

    public void AddTextAsCriticalHit()
    {
        AddTextLines("It was a Critical Hit!");
    }
    public void AddTextAsStatusEffect(string id, string statusAilment)
    {
        AddTextLines(id + " was affected by " + statusAilment + ".");
    }

    public void AddTextAsStatusEffectWornOff(string id, string statusAilment)
    {
        AddTextLines(statusAilment + " has worn off " + id + ".");
    }

    public void AddTextAsUseable(string id, string useableName)
    {
        AddTextLines(useableName + " has been used on " + id + ".");
    }

    public void AddTextAsCannotRevive(string itemName, string userToHealId)
    {
        AddTextLines(itemName + " cannot revive " + userToHealId + ".");
    }

    public void AddTextAsAlreadyMaxHealth(string userToHealId)
    {
        AddTextLines(userToHealId + " is already at max health.");
    }

    public void AddTextAsNotEnoughMana(string userId)
    {
        AddTextLines(userId + " does not have enough MP for this.");
    }

    private void ReturnToPreviousState()
    {
        Enum s = PreviousState;
        PreviousState = null;
        battleStateMachine.ReturnBackToState(s);
    }
}
