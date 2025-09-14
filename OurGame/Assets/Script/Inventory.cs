using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton
    public static Inventory Instance { get; private set; }

    public event Action<UpgradeData> OnItemAdded;
    public event Action<UpgradeData> OnItemEquipped;

    private List<UpgradeData> items = new List<UpgradeData>();
    private List<UpgradeData> equippedItems = new List<UpgradeData>();

    public int maxSlots = 6;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddItem(UpgradeData item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.LogWarning("Inventory is full!");
            return false;
        }

        items.Add(item);
        OnItemAdded?.Invoke(item);
        return true;
    }

    public List<UpgradeData> GetItems()
    {
        return items;
    }

    public void EquipItem(UpgradeData item)
    {
        if (items.Contains(item) && !equippedItems.Contains(item))
        {
            equippedItems.Add(item);
            OnItemEquipped?.Invoke(item);
            Debug.Log($"Equipped: {item.upgradeName}");
        }
        else
        {
            Debug.LogWarning("Cannot equip item. It is not in the inventory or already equipped.");
        }
    }

    public List<UpgradeData> GetEquippedItems()
    {
        return equippedItems;
    }

    public void UnequipItem(UpgradeData item)
    {
        if (equippedItems.Contains(item))
        {
            equippedItems.Remove(item);
        }
    }
}