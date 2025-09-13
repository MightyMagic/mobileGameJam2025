using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    // Структура для описания карты улучшения
    [System.Serializable]
    public struct UpgradeCard
    {
        public string title;
        public string description;
        public int cost;
        public UpgradeType type;
        public float value;
    }

    public enum UpgradeType
    {
        PlayerBaseHealth,
        TurretDamage,
        TowerDefense,
        // Добавьте другие типы улучшений здесь
    }

    [Header("Upgrade Settings")]
    public List<UpgradeCard> availableUpgrades; // Список всех возможных улучшений
    public float upgradeThreshold = 50f; // Сколько очков нужно для предложения улучшений

    [Header("UI Elements")]
    public GameObject upgradePanel;
    public TextMeshProUGUI[] cardTitles;
    public TextMeshProUGUI[] cardDescriptions;
    public TextMeshProUGUI[] cardCosts;

    private bool isPanelActive = false;
    private List<UpgradeCard> currentCards = new List<UpgradeCard>();

    void Start()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
    }

    void Update()
    {
        // Проверяем, достаточно ли у игрока очков и неактивна ли уже панель
        if (GameManager.ChoicePoints >= upgradeThreshold && !isPanelActive)
        {
            ShowUpgradePanel();
        }
    }

    private void ShowUpgradePanel()
    {
        isPanelActive = true;
        Time.timeScale = 0; // Останавливаем игру
        upgradePanel.SetActive(true);

        // Выбираем 3 случайные карты
        currentCards = GetRandomUpgrades(3);

        // Обновляем UI
        for (int i = 0; i < currentCards.Count; i++)
        {
            cardTitles[i].text = currentCards[i].title;
            cardDescriptions[i].text = currentCards[i].description;
            cardCosts[i].text = "Стоимость: " + currentCards[i].cost;
        }
    }

    // Этот метод вызывается при нажатии кнопки на UI
    public void SelectUpgrade(int index)
    {
        UpgradeCard selected = currentCards[index];
        if (GameManager.ChoicePoints >= selected.cost)
        {
            GameManager.AddChoicePoints(-selected.cost); // Вычитаем стоимость
            ApplyUpgrade(selected);
            HideUpgradePanel();
        }
    }

    private void ApplyUpgrade(UpgradeCard card)
    {
        // Здесь мы будем применять эффект улучшения
        switch (card.type)
        {
            case UpgradeType.PlayerBaseHealth:
                // Увеличиваем здоровье базы
                PlayerBase playerBase = FindObjectOfType<PlayerBase>();
                if (playerBase != null)
                {
                    playerBase.maxHealth += card.value;
                    playerBase.currentHealth += card.value;
                    // Также обновляем UI
                    playerBase.healthSlider.maxValue = playerBase.maxHealth;
                    playerBase.healthSlider.value = playerBase.currentHealth;
                }
                break;
            case UpgradeType.TurretDamage:
                // Увеличиваем урон всех пушек
                Projectile[] projectiles = FindObjectsOfType<Projectile>();
                foreach (var proj in projectiles)
                {
                    proj.damage += card.value;
                }
                // Можно сделать то же самое для других типов урона (Rocket, Flamethrower и т.д.)
                break;
                // Добавьте другие случаи здесь
        }
        Debug.Log($"Applied upgrade: {card.title}");
    }

    private List<UpgradeCard> GetRandomUpgrades(int count)
    {
        List<UpgradeCard> randomUpgrades = new List<UpgradeCard>();
        List<UpgradeCard> tempUpgrades = new List<UpgradeCard>(availableUpgrades); // Копируем список

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
        isPanelActive = false;
        Time.timeScale = 1; // Возобновляем игру
        upgradePanel.SetActive(false);
        // Сбрасываем счетчик, чтобы выбор не появлялся снова сразу
        GameManager.ChoicePoints = 0;
    }
}