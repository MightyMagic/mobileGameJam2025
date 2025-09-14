using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Settings")]
    public List<UpgradeData> availableUpgrades; // Все возможные апгрейды
    public int upgradeThreshold = 50; // Сколько очков нужно для предложения улучшений

    [Header("UI Elements")]
    public GameObject upgradePanel;
    public TextMeshProUGUI[] cardTitles;
    public TextMeshProUGUI[] cardDescriptions;
    public TextMeshProUGUI[] cardCosts;
    public UnityEngine.UI.Image[] cardIcons; // Ссылка на иконки на самих картах

    private List<UpgradeData> currentCards = new List<UpgradeData>();
    private bool hasUpgradesAvailable = false;

    [Header("Weapons on the player")]
    [SerializeField] FlameThrowerWeapon flameThrower;

    void OnEnable()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }

        //GameManager.OnBuildPhaseStart += CheckForUpgrades;
    }

    private void OnDisable()
    {
        GameManager.OnBuildPhaseStart -= CheckForUpgrades;
    }


    public void CheckForUpgrades()
    {
        if (GameManager.Instance.ChoicePoints >= upgradeThreshold && !hasUpgradesAvailable)
        {
            // Player has points and we haven't shown the panel yet.
            // Show the panel and PAUSE the game. Do NOT start the build phase.
            ShowUpgradePanel();
            hasUpgradesAvailable = true;
        }
        else
        {
            // Player has no points OR the panel was already dealt with.
            // Proceed immediately to the build phase.
            BuildManager.Instance.EnableBuilding();
        }
    }

    public void ShowUpgradePanel()
    {
        upgradePanel.SetActive(true);
        GameManager.Instance.PauseGame();


        currentCards = GetRandomUpgrades(3);

        for (int i = 0; i < currentCards.Count; i++)
        {
            cardTitles[i].text = currentCards[i].upgradeName;
            cardDescriptions[i].text = currentCards[i].description;
            cardCosts[i].text = "Стоимость: " + currentCards[i].cost;

            // Загружаем иконку для карты
            cardIcons[i].sprite = currentCards[i].cardIcon;
        }
    }

    public void SelectUpgrade(int index)
    {
        UpgradeData selected = currentCards[index];

        if (GameManager.Instance.ChoicePoints >= selected.cost)
        {
            GameManager.Instance.AddChoicePoints(-selected.cost);
            ApplyUpgrade(selected);

            // Добавляем выбранный апгрейд в инвентарь
            Inventory.Instance.AddItem(selected);

            HideUpgradePanel();
        }
    }

    private void ApplyUpgrade(UpgradeData card)
    {
        switch (card.type)
        {
            case UpgradeType.Equipment_Flamethrower:
                Debug.Log("Применено улучшение: Огнемёт.");

                flameThrower.ApplyUpgradeStats(card);

                // Здесь добавьте логику, которая дает игроку огнемёт или улучшает его
                break;
            case UpgradeType.Equipment_Saw:
                Debug.Log("Применено улучшение: Пила.");
                // Здесь добавьте логику, которая дает игроку пилу или улучшает её
                break;
            case UpgradeType.Equipment_MachineGun:
                Debug.Log("Применено улучшение: Пулемёт.");
                // Здесь добавьте логику, которая дает игроку пулемёт или улучшает его
                break;
            case UpgradeType.Equipment_Rocket:
                Debug.Log("Применено улучшение: Ракетная установка.");
                // Здесь добавьте логику, которая дает игроку ракетную установку или улучшает её
                break;
            case UpgradeType.Rail:
                Debug.Log("Применено улучшение: Рельсы.");
                // Здесь добавьте логику, которая улучшает рельсы, по которым ездят враги
                break;
            default:
                Debug.LogWarning($"Неизвестный тип улучшения: {card.type}");
                break;
        }
        Debug.Log($"Applied upgrade: {card.upgradeName}");
    }

    private List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> possibleUpgrades = new List<UpgradeData>();

        // 1. Проверяем все доступные апгрейды
        foreach (var upgrade in availableUpgrades)
        {
            switch (upgrade.type)
            {
                // Категория 1: Новая пушка (если её ещё нет и есть место)
                case UpgradeType.Equipment_Flamethrower:
                case UpgradeType.Equipment_Saw:
                case UpgradeType.Equipment_MachineGun:
                case UpgradeType.Equipment_Rocket:
                    // Проверяем, есть ли у игрока уже такая пушка
                    if (!PlayerHasEquipment(upgrade.type))
                    {
                        // Проверяем, есть ли место в инвентаре
                        if (Inventory.Instance.GetItems().Count < Inventory.Instance.maxSlots)
                        {
                            possibleUpgrades.Add(upgrade);
                        }
                    }
                    break;

                // Категория 2: Апгрейд пушки
                case UpgradeType.Upgrade_Flamethrower:
                case UpgradeType.Upgrade_Saw:
                case UpgradeType.Upgrade_MachineGun:
                case UpgradeType.Upgrade_Rocket:
                    // Проверяем, есть ли у игрока соответствующая пушка
                    if (PlayerHasEquipment(GetBaseEquipmentType(upgrade.type)))
                    {
                        possibleUpgrades.Add(upgrade);
                    }
                    break;

                // Категория 3: Рельсы (всегда доступны)
                case UpgradeType.Rail:
                    possibleUpgrades.Add(upgrade);
                    break;
            }
        }

        // 2. Выбираем случайные карты из списка возможных
        List<UpgradeData> finalCards = new List<UpgradeData>();
        for (int i = 0; i < count; i++)
        {
            if (possibleUpgrades.Count == 0) break;

            int randomIndex = Random.Range(0, possibleUpgrades.Count);
            finalCards.Add(possibleUpgrades[randomIndex]);
            possibleUpgrades.RemoveAt(randomIndex); // Удаляем, чтобы не выбрать дважды
        }

        return finalCards;
    }

    // Новые вспомогательные методы
    private bool PlayerHasEquipment(UpgradeType type)
    {
        // Нужно как-то отслеживать, какие пушки есть у игрока.
        // Предлагаю создать в Inventory или GameManager список, который будет хранить
        // типы уже полученных пушек.
        // Например: GameManager.Instance.OwnedEquipment.Contains(type);
        return false; // Замените на вашу логику
    }

    // Этот метод преобразует тип апгрейда в тип базовой пушки (например, Upgrade_Flamethrower -> Equipment_Flamethrower)
    private UpgradeType GetBaseEquipmentType(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.Upgrade_Flamethrower:
                return UpgradeType.Equipment_Flamethrower;
            case UpgradeType.Upgrade_Saw:
                return UpgradeType.Equipment_Saw;
            case UpgradeType.Upgrade_MachineGun:
                return UpgradeType.Equipment_MachineGun;
            case UpgradeType.Upgrade_Rocket:
                return UpgradeType.Equipment_Rocket;
            default:
                return upgradeType;
        }
    }

    private void HideUpgradePanel()
    {
        GameManager.Instance.ResumeGame();
        upgradePanel.SetActive(false);
        hasUpgradesAvailable = false;

        BuildManager.Instance.EnableBuilding();
    }
}