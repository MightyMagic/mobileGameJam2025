// Inventory.cs (обновленный код)
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
    private UpgradeData equippedItem;

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

        // Предмет сначала добавляется в инвентарь
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
        if (items.Contains(item))
        {
            equippedItem = item;
            OnItemEquipped?.Invoke(equippedItem);
            Debug.Log($"Equipped: {item.upgradeName}");
        }
        else
        {
            Debug.LogWarning("Cannot equip item. It is not in the inventory.");
        }
    }

    public UpgradeData GetEquippedItem()
    {
        return equippedItem;
    }
}