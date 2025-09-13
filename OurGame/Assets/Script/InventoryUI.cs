using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Tooltip("Список UI-объектов, представляющих слоты инвентаря. Заполняется вручную.")]
    public List<Image> inventorySlots;

    [Tooltip("Список UI-объектов, представляющих иконки предметов внутри слотов.")]
    public List<Image> itemIcons;

    private void Start()
    {
        // Убедитесь, что количество UI-слотов соответствует максимальному количеству слотов в инвентаре
        if (inventorySlots.Count != Inventory.Instance.maxSlots || itemIcons.Count != Inventory.Instance.maxSlots)
        {
            Debug.LogError("Количество слотов в UI не совпадает с размером инвентаря!");
            return;
        }

        // Подписываемся на событие добавления предмета в инвентарь
        Inventory.Instance.OnItemAdded += OnItemAdded;

        // При старте скрываем все иконки предметов, чтобы слоты были пустыми
        foreach (var icon in itemIcons)
        {
            icon.enabled = false;
        }

        // Обновляем UI на случай, если инвентарь уже содержит предметы
        UpdateUI();
    }

    private void OnDestroy()
    {
        // Отписываемся от события, чтобы избежать ошибок, если объект UI будет уничтожен
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnItemAdded -= OnItemAdded;
        }
    }

    private void OnItemAdded(UpgradeData item)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        List<UpgradeData> currentItems = Inventory.Instance.GetItems();

        for (int i = 0; i < itemIcons.Count; i++)
        {
            if (i < currentItems.Count)
            {
                // Если в рюкзаке есть предмет, показываем его иконку из UpgradeData
                itemIcons[i].sprite = currentItems[i].inventoryIcon;
                itemIcons[i].enabled = true; // Делаем иконку видимой
            }
            else
            {
                // Если слоты пустые, скрываем иконки
                itemIcons[i].enabled = false;
            }
        }
    }
}