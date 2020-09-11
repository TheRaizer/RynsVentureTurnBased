using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxHandler
{
    public Enum PreviousState { get; set; } = null;

    private readonly MenusHandler menusHandler;
    private readonly BattleLogic battleLogic;
    private readonly StateMachine battleStateMachine;

    private readonly WaitForSeconds writeTime = new WaitForSeconds(0.08f);
    private int currentLine = 0;
    private bool Finished;
    private bool running;
    private bool skip;
    private readonly List<string> textLines = new List<string>();

    public TextBoxHandler(MenusHandler _menusHandler, BattleLogic _battleLogic, StateMachine _battleStateMachine)
    {
        menusHandler = _menusHandler;
        battleLogic = _battleLogic;
        battleStateMachine = _battleStateMachine;
    }

    public void InputUpdate()
    {
        if (running && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            skip = true;
        }
        if (!running && !Finished && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            menusHandler.StartCoroutine(BuildMultiStringTextCo());
        }
        if (Finished && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            ResetTextBox();
            Finished = false;
            if(PreviousState != null)
            {
                ReturnToPreviousState();
            }
            else
                battleLogic.CalculateNextTurn();
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

    private void ReturnToPreviousState()
    {
        Enum s = PreviousState;
        PreviousState = null;
        battleStateMachine.ReturnBackToState(s);
    }
    public void GenerateEnemyText(List<EntityActionInfo> attackInfos, EntityAction attackToUse)
    {
        for (int i = 0; i < attackInfos.Count; i++)
        {
            AddTextAsAttack(battleLogic.CurrentEnemy.Id, attackToUse.AttackText, attackInfos[i].targetId);
            if (!attackInfos[i].hitTarget)
            {
                AddTextOnMiss(battleLogic.CurrentEnemy.Id, attackInfos[i].targetId);
            }
            else if (attackToUse.WasCriticalHit)
            {
                AddTextAsCriticalHit();
            }
        }
    }
}
