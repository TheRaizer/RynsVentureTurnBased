using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStorageForArea : MonoBehaviour
{
    [System.Serializable]
    public class EnemyForDictionary
    {
        public Areas area;
        public List<GameObject> enemyObjects;
    }

    [SerializeField] private List<EnemyForDictionary> enemies = null;
    public Dictionary<Areas, List<GameObject>> EnemiesDic { get; private set; } = new Dictionary<Areas, List<GameObject>>();

    private void Awake()
    {
        foreach(EnemyForDictionary e in enemies)
        {
            EnemiesDic.Add(e.area, e.enemyObjects);
        }
    }
}
