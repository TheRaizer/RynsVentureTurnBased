using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnActionPopup : MonoBehaviour
{
    public List<Vector2> LocationsToSpawn { private get; set; }
    public List<int> Amount  { private get; set; }
    public List<bool> Critical { private get; set; }
    public bool Support { private get; set; }
    public List<bool> TargetHits { private get; set; }

    private ObjectPooler objectPooler;

    private void Awake()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    private void SpawnText()//method used during animations
    {
        for (int i = 0; i < LocationsToSpawn.Count; i++)
        {
            GameObject g = objectPooler.SpawnFromPool("ActionPopup", Vector2.zero, Quaternion.identity);

            RectTransform r = g.GetComponent<RectTransform>();

            if (!TargetHits[i])
            {
                g.GetComponent<TextMeshProUGUI>().text = "Miss";
            }
            else
                g.GetComponent<TextMeshProUGUI>().text = Amount[i].ToString();

            r.anchoredPosition = LocationsToSpawn[i];
        }
    }
}
