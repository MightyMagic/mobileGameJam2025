using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton
    public static Inventory Instance { get; private set; }

    public event Action<Item> OnItemAdded;

    private List<Item> items = new List<Item>();
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

    public bool AddItem(Item item)
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

    public List<Item> GetItems()
    {
        return items;
    }
}

// Базовый класс для предметов
[Serializable]
public class Item
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
}