using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnActionPopup : MonoBehaviour
{
    public List<Vector2> LocationsToSpawn { private get; set; }
    public int Amount  { private get; set; }
    public bool Critical { private get; set; }
    public bool Support { private get; set; }
    public bool HitTarget { private get; set; }

    private ObjectPooler objectPooler;

    private void Awake()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

    private void SpawnText()
    {
        for (int i = 0; i < LocationsToSpawn.Count; i++)
        {
            GameObject g = objectPooler.SpawnFromPool("ActionPopup", Vector2.zero, Quaternion.identity);

            RectTransform r = g.GetComponent<RectTransform>();

            if (!HitTarget)
            {
                g.GetComponent<TextMeshProUGUI>().text = "Miss";
            }
            else
                g.GetComponent<TextMeshProUGUI>().text = Amount.ToString();

            r.anchoredPosition = LocationsToSpawn[i];
        }
    }
}
