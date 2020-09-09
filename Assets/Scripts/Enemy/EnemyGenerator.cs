using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGenerator
{
    public Areas CurrentArea { get; private set; } = Areas.Forest;
    private readonly EnemyStorageForArea enemyStorage;
    private readonly BattleLogic battleLogic;
    private readonly char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E' };
    public EnemyGenerator(EnemyStorageForArea _enemyStorage, BattleLogic _battleLogic)
    {
        enemyStorage = _enemyStorage;
        battleLogic = _battleLogic;
    }

    public void InstantiateEnemies()
    {
        int numOfEnemies = UnityEngine.Random.Range(1, 6);

        for (int i = 0; i < numOfEnemies; i++)
        {
            int randomAreaEnemy = UnityEngine.Random.Range(0, enemyStorage.EnemiesDic[CurrentArea].Count);
            GameObject chosenEnemy = UnityEngine.Object.Instantiate(enemyStorage.EnemiesDic[CurrentArea][randomAreaEnemy]);

            string chosenEnemyId = chosenEnemy.GetComponent<Enemy>().Id;
            chosenEnemy.GetComponent<Enemy>().Id = chosenEnemyId + " " + letters[CheckForDuplicateEnemies(chosenEnemyId)];

            battleLogic.Enemies[i] = chosenEnemy;
            battleLogic.AttackableEnemies.Add(battleLogic.Enemies[i].GetComponent<Enemy>().Stats);
            battleLogic.TotalExpFromBattle += battleLogic.Enemies[i].GetComponent<Enemy>().ExpOnDeath;
        }

        OrderEnemies();
    }

    private void OrderEnemies()
    {
        List<GameObject> gL = battleLogic.Enemies.ToList();
        for (int i = gL.Count - 1; i >= 0; i--)//start at the end and move to 0 cuz the count is constantly being reduced
        {
            if (gL[i] == null)
            {
                gL[i] = gL[gL.Count - 1];//swap the last element in the list with the current null
                gL.RemoveAt(gL.Count - 1);//remove the last element
            }
        }

        gL = gL.OrderBy(x => x.GetComponent<Enemy>().Id).ToList();//order the list by id

        for (int i = 0; i < battleLogic.Enemies.Length; i++)//copy the list to the array of enemies
        {
            if (i >= gL.Count)
            {
                battleLogic.Enemies[i] = null;
            }
            else
            {
                battleLogic.Enemies[i] = gL[i];
            }
        }

        List<StatsManager> attackableEnemies = battleLogic.AttackableEnemies.OrderBy(x => x.user.Id).ToList();//order the attackable enemies
        battleLogic.AttackableEnemies = attackableEnemies;
    }

    private int CheckForDuplicateEnemies(string id)
    {
        int duplicateCount = 0;
        foreach(GameObject g in battleLogic.Enemies)
        {
            if (g == null) continue;

            Enemy e = g.GetComponent<Enemy>();
            string enemyFullId = e.Id;

            if (enemyFullId.Length < id.Length) continue;

            string enemyBaseId = enemyFullId.Substring(0, id.Length);

            if(enemyBaseId == id)
            {
                duplicateCount++;
            }
        }
        return duplicateCount;
    }
}
