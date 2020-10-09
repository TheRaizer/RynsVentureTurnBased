using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string id;
        public int amount;
        public GameObject prefab;
        public GameObject parentObject;
        public Queue<GameObject> gameObjects = new Queue<GameObject>();
    }

    [SerializeField] private List<Pool> poolsToGenerate;
    private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    public void Awake()
    {
        foreach(Pool p in poolsToGenerate)
        {
            for (int i = 0; i < p.amount; i++)
            {
                GameObject g = Instantiate(p.prefab);
                if(p.parentObject != null)
                {
                    g.transform.SetParent(p.parentObject.transform);
                }
                g.SetActive(false);
                p.gameObjects.Enqueue(g);
            }
            pools.Add(p.id, p);
        }
    }

    public GameObject SpawnFromPool(string id, Vector2 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(id))
        {
            Debug.LogError("Pool does not contain the key " + id);
            return null;
        }
        Pool p = pools[id];

        GameObject g = p.gameObjects.Dequeue();

        g.transform.position = position;
        g.transform.rotation = rotation;

        g.SetActive(true);
        p.gameObjects.Enqueue(g);

        return g;
    }
}
