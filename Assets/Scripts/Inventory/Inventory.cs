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

    public void PrintCurrentInventoryWorldText()
    {
        for (int i = 0; i < InventoryDic[CurrentInventoryOpen].Count; i++)
        {
            TextMeshProUGUI textMesh = worldMenusHandler.TextBoxes[i].GetComponent<TextMeshProUGUI>();

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

    public void UseItemInventoryInWorld(int indexToRemove, StatsManager entityToUseOn)
    {
        Useable itemToUse = (Useable)InventoryDic[typeof(Useable)][indexToRemove];
        if (InventoryDic[typeof(Useable)].Contains(itemToUse))
        {
            itemToUse.OnUseInWorld(entityToUseOn);

            if(itemToUse.IsEmpty)
            {
                InventoryDic[typeof(Useable)].Remove(itemToUse);
            }
        }
        else
        {
            Debug.Log("NO item in inventory");
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
