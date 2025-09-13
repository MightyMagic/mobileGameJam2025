using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    // ��������� ��� �������� ����� ���������
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
        // �������� ������ ���� ��������� �����
    }

    [Header("Upgrade Settings")]
    public List<UpgradeCard> availableUpgrades; // ������ ���� ��������� ���������
    public float upgradeThreshold = 50f; // ������� ����� ����� ��� ����������� ���������

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
        // ���������, ���������� �� � ������ ����� � ��������� �� ��� ������
        if (GameManager.ChoicePoints >= upgradeThreshold && !isPanelActive)
        {
            ShowUpgradePanel();
        }
    }

    private void ShowUpgradePanel()
    {
        isPanelActive = true;
        Time.timeScale = 0; // ������������� ����
        upgradePanel.SetActive(true);

        // �������� 3 ��������� �����
        currentCards = GetRandomUpgrades(3);

        // ��������� UI
        for (int i = 0; i < currentCards.Count; i++)
        {
            cardTitles[i].text = currentCards[i].title;
            cardDescriptions[i].text = currentCards[i].description;
            cardCosts[i].text = "���������: " + currentCards[i].cost;
        }
    }

    // ���� ����� ���������� ��� ������� ������ �� UI
    public void SelectUpgrade(int index)
    {
        UpgradeCard selected = currentCards[index];
        if (GameManager.ChoicePoints >= selected.cost)
        {
            GameManager.AddChoicePoints(-selected.cost); // �������� ���������
            ApplyUpgrade(selected);
            HideUpgradePanel();
        }
    }

    private void ApplyUpgrade(UpgradeCard card)
    {
        // ����� �� ����� ��������� ������ ���������
        switch (card.type)
        {
            case UpgradeType.PlayerBaseHealth:
                // ����������� �������� ����
                PlayerBase playerBase = FindObjectOfType<PlayerBase>();
                if (playerBase != null)
                {
                    playerBase.maxHealth += card.value;
                    playerBase.currentHealth += card.value;
                    // ����� ��������� UI
                    playerBase.healthSlider.maxValue = playerBase.maxHealth;
                    playerBase.healthSlider.value = playerBase.currentHealth;
                }
                break;
            case UpgradeType.TurretDamage:
                // ����������� ���� ���� �����
                Projectile[] projectiles = FindObjectsOfType<Projectile>();
                foreach (var proj in projectiles)
                {
                    proj.damage += card.value;
                }
                // ����� ������� �� �� ����� ��� ������ ����� ����� (Rocket, Flamethrower � �.�.)
                break;
                // �������� ������ ������ �����
        }
        Debug.Log($"Applied upgrade: {card.title}");
    }

    private List<UpgradeCard> GetRandomUpgrades(int count)
    {
        List<UpgradeCard> randomUpgrades = new List<UpgradeCard>();
        List<UpgradeCard> tempUpgrades = new List<UpgradeCard>(availableUpgrades); // �������� ������

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
        Time.timeScale = 1; // ������������ ����
        upgradePanel.SetActive(false);
        // ���������� �������, ����� ����� �� ��������� ����� �����
        GameManager.ChoicePoints = 0;
    }
}