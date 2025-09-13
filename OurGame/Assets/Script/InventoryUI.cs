using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Image> inventorySlots; // Список слотов, который мы заполним в инспекторе

    // Ссылка на компонент спрайта самого предмета, внутри каждого слота.
    // Если у вас в слоте только одно изображение, то это не нужно.
    // Если же у вас есть рамка, а внутри нее иконка, то нам нужна ссылка на иконку.
    public List<Image> itemIcons;

    void Start()
    {
        // Подписываемся на событие добавления предмета
        Inventory.Instance.OnItemAdded += OnItemAdded;

        // При старте скрываем все иконки предметов, чтобы слоты были пустыми
        foreach (var icon in itemIcons)
        {
            icon.enabled = false;
        }
    }

    private void OnItemAdded(Item item)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        List<Item> currentItems = Inventory.Instance.GetItems();

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < currentItems.Count)
            {
                // Если в рюкзаке есть предмет, показываем его
                itemIcons[i].sprite = currentItems[i].itemIcon;
                itemIcons[i].enabled = true; // Делаем иконку видимой
            }
            else
            {
                // Если слоты пустые, скрываем их
                itemIcons[i].enabled = false;
            }
        }
    }
}