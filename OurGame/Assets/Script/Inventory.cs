using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton
    public static Inventory Instance { get; private set; }

    public event Action<UpgradeData> OnItemAdded;

    private List<UpgradeData> items = new List<UpgradeData>();
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
        OnItemAdded?.Invoke(item); // Уведомляем подписчиков о добавлении предмета
        return true;
    }

    public List<UpgradeData> GetItems()
    {
        return items;
    }
}