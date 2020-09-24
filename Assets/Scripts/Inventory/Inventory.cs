using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject hpPotionPrefab = null;
    [SerializeField] private GameObject attackUpPrefab = null;

    public Dictionary<Type, IList> InventoryDic { get; private set; } = new Dictionary<Type, IList>();
    private WorldMenusHandler worldMenusHandler;

    public Type CurrentInventoryOpen { get; set; } = typeof(Useable);

    private void Awake()
    {
        worldMenusHandler = GetComponent<WorldMenusHandler>();
        InventoryDic.Add(typeof(Useable), new List<Useable>());

        Useable hpPotion_1 = hpPotionPrefab.GetComponent<SmallHpPotion>().ShallowClone();
        Useable hpPotion_2 = hpPotionPrefab.GetComponent<SmallHpPotion>().ShallowClone();
        Useable attackUp = attackUpPrefab.GetComponent<SmallAttackUp>().ShallowClone();
        hpPotion_1.Amount += 15;
        hpPotion_2.Amount = 1;
        attackUp.Amount = 1;
        AddToInventory(attackUp);
        AddToInventory(hpPotion_1);
        AddToInventory(hpPotion_2);
        RemoveFromInventory(1, 1, hpPotion_1.GetType().BaseType);
    }

    public void PrintCurrentInventoryText(GameObject[] textBoxes)
    {
        for (int i = 0; i < InventoryDic[CurrentInventoryOpen].Count; i++)
        {
            TextMeshProUGUI textMesh = textBoxes[i].GetComponent<TextMeshProUGUI>();

            Item item = (Item)InventoryDic[CurrentInventoryOpen][i];

            textMesh.text = item.Amount + " " + item.Id;
        }
    }

    public void AddToInventory<T>(T itemToAdd)
    {
        IStoreable item = (IStoreable)itemToAdd;
        Type t = itemToAdd.GetType();
        IList inventory = InventoryDic[t.BaseType];

        if (inventory.Contains(itemToAdd))
        {
            int index = inventory.IndexOf(itemToAdd);
            IStoreable storeable = (IStoreable)inventory[index];

            if(storeable.Amount + item.Amount >= storeable.MaxHoldable)
            {
                Debug.Log("Cant carry any more");
            }
            else
                storeable.Amount += item.Amount;
        }
        else
        {
            inventory.Add(item);
        }
    }

    public void UseItemInventoryInRoam(int indexToUse, StatsManager statsToHeal, List<StatsManager> friendlyStats)
    {
        Useable itemToUse = (Useable)InventoryDic[typeof(Useable)][indexToUse];
        itemToUse.OnUseInRoam(statsToHeal, friendlyStats);

        if(itemToUse.IsEmpty)
        {
            InventoryDic[typeof(Useable)].Remove(itemToUse);
        }
    }

    public void UseItemInventoryInBattle(int indexToUse, StatsManager statsToHeal, List<StatsManager> friendlyStats, StateMachine battleStateMachine, BattleTextBoxHandler battleTexBoxHandler)
    {
        Useable itemToUse = (Useable)InventoryDic[typeof(Useable)][indexToUse];

        itemToUse.OnUseInBattle(statsToHeal, friendlyStats, battleStateMachine, battleTexBoxHandler);

        if (itemToUse.IsEmpty)
        {
            InventoryDic[typeof(Useable)].Remove(itemToUse);
        }
    }

    public void RemoveFromInventory(int amount, int indexToRemove, Type inventoryType)
    {
        IList inventoryToRemoveFrom = InventoryDic[inventoryType];

        IStoreable item = (IStoreable)inventoryToRemoveFrom[indexToRemove];
        if (item.Amount - amount <= 0)
        {
            inventoryToRemoveFrom.Remove(inventoryToRemoveFrom[indexToRemove]);
        }
        else
        {
            item.Amount -= amount;
        }
    }
}

public interface IStoreable
{
    int Amount { get; set; }
    int MaxHoldable { get; }
}
