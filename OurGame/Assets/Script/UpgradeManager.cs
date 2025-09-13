using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    void Start()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }

        GameManager.OnBuildPhaseStart += CheckForUpgrades;
    }

    private void OnDestroy()
    {
        GameManager.OnBuildPhaseStart -= CheckForUpgrades;
    }

    private void CheckForUpgrades()
    {
        if (GameManager.Instance.ChoicePoints >= upgradeThreshold && !hasUpgradesAvailable)
        {
            ShowUpgradePanel();
            hasUpgradesAvailable = true;
        }
    }

    public void ShowUpgradePanel()
    {
        GameManager.Instance.PauseGame();
        upgradePanel.SetActive(true);

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
        List<UpgradeData> randomUpgrades = new List<UpgradeData>();
        List<UpgradeData> tempUpgrades = new List<UpgradeData>(availableUpgrades);

        for (int i = 0; i < count; i++)
        {
            if (tempUpgrades.Count == 0) break;
            int randomIndex = Random.Range(0, tempUpgrades.Count);
            randomUpgrades.Add(tempUpgrades[randomIndex]);
            tempUpgrades.RemoveAt(randomIndex);
        }
        return randomUpgrades;
    }

    private void HideUpgradePanel()
    {
        GameManager.Instance.ResumeGame();
        upgradePanel.SetActive(false);
        hasUpgradesAvailable = false;
    }
}