using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    // A structure to define an upgrade card
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
        // Add other upgrade types here
    }

    [Header("Upgrade Settings")]
    public List<UpgradeCard> availableUpgrades; // All possible upgrades
    public int upgradeThreshold = 50; // Points required to trigger an upgrade choice

    [Header("UI Elements")]
    public GameObject upgradePanel;
    public TextMeshProUGUI[] cardTitles;
    public TextMeshProUGUI[] cardDescriptions;
    public TextMeshProUGUI[] cardCosts;

    private List<UpgradeCard> currentCards = new List<UpgradeCard>();
    private bool hasUpgradesAvailable = false;

    void Start()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }

        // We subscribe to the build phase start event
        GameManager.OnBuildPhaseStart += CheckForUpgrades;
    }

    private void OnDestroy()
    {
        // Always unsubscribe to prevent errors when the object is destroyed
        GameManager.OnBuildPhaseStart -= CheckForUpgrades;
    }

    private void CheckForUpgrades()
    {
        // Check if the player has enough points and hasn't already been offered an upgrade this round
        if (GameManager.Instance.ChoicePoints >= upgradeThreshold && !hasUpgradesAvailable)
        {
            ShowUpgradePanel();
            hasUpgradesAvailable = true; // Mark that an upgrade is available
        }
    }

    public void ShowUpgradePanel()
    {
        GameManager.Instance.PauseGame(); // Pause the game when the panel is active
        upgradePanel.SetActive(true);

        // Select 3 random cards
        currentCards = GetRandomUpgrades(3);

        // Update the UI
        for (int i = 0; i < currentCards.Count; i++)
        {
            cardTitles[i].text = currentCards[i].title;
            cardDescriptions[i].text = currentCards[i].description;
            cardCosts[i].text = "Стоимость: " + currentCards[i].cost;
        }
    }

    // This method is called when a UI button is pressed
    public void SelectUpgrade(int index)
    {
        UpgradeCard selected = currentCards[index];

        if (GameManager.Instance.ChoicePoints >= selected.cost)
        {
            GameManager.Instance.AddChoicePoints(-selected.cost); // Subtract the cost
            ApplyUpgrade(selected);
            HideUpgradePanel();
        }
    }

    private void ApplyUpgrade(UpgradeCard card)
    {
        // Apply the upgrade effect
        switch (card.type)
        {
            case UpgradeType.PlayerBaseHealth:
                // Increase base health
                PlayerBase playerBase = FindObjectOfType<PlayerBase>();
                if (playerBase != null)
                {
                    playerBase.maxHealth += card.value;
                    playerBase.currentHealth += card.value;
                }
                break;
            case UpgradeType.TurretDamage:
                // Increase damage of all turrets
                // Note: You'll need to update this to handle different turret types
                Projectile[] projectiles = FindObjectsOfType<Projectile>();
                foreach (var proj in projectiles)
                {
                    proj.damage += card.value;
                }
                break;
        }
        Debug.Log($"Applied upgrade: {card.title}");
    }

    private List<UpgradeCard> GetRandomUpgrades(int count)
    {
        List<UpgradeCard> randomUpgrades = new List<UpgradeCard>();
        List<UpgradeCard> tempUpgrades = new List<UpgradeCard>(availableUpgrades);

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
        GameManager.Instance.ResumeGame(); // Resume the game
        upgradePanel.SetActive(false);
        hasUpgradesAvailable = false; // Reset the flag so a new upgrade can be offered later
    }
}