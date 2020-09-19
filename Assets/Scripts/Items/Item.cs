using UnityEngine;

public class Item : MonoBehaviour, IStoreable
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public int MaxHoldable { get; private set; }

    [SerializeField] private float dropChance = 50;
    public int Amount { get; set; } = 1;
}