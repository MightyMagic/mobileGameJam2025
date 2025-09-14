using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Upgrade Settings")]
    public List<UpgradeData> availableUpgrades;
    public int upgradeThreshold = 50;

    [Header("UI Elements")]
    public GameObject upgradePanel;
    public TextMeshProUGUI[] cardTitles;
    public TextMeshProUGUI[] cardDescriptions;
    public UnityEngine.UI.Image[] cardIcons;

    private List<UpgradeData> currentCards = new List<UpgradeData>();
    private bool hasUpgradesAvailable = false;

    [Header("Weapon Component References")]
    public FlameThrowerWeapon flameThrowerWeapon;

    private void Awake()
    {
       if (Instance == null)
       {
           Instance = this;
       }
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    void Start()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
    }

    public void CheckForUpgrades()
    {
        if (GameManager.Instance.ChoicePoints >= upgradeThreshold && !hasUpgradesAvailable)
        {
            ShowUpgradePanel();
            hasUpgradesAvailable = true;
        }
        else
        {
            GameManager.Instance.BeginBuildPhase();
        }
    }

    public void ShowUpgradePanel()
    {
        upgradePanel.SetActive(true);
        currentCards = GetRandomUpgrades(3);

        for (int i = 0; i < currentCards.Count; i++)
        {
            if (cardTitles[i] != null) cardTitles[i].text = currentCards[i].upgradeName;
            if (cardDescriptions[i] != null) cardDescriptions[i].text = currentCards[i].description;
            if (cardIcons[i] != null) cardIcons[i].sprite = currentCards[i].cardIcon;
        }
    }

    public void SelectUpgrade(int index)
    {
        UpgradeData selected = currentCards[index];

        if (GameManager.Instance.ChoicePoints >= selected.cost)
        {
            GameManager.Instance.AddChoicePoints(-selected.cost);
            ApplyUpgrade(selected);

            // Inventory.Instance.AddItem(selected); // Добавьте, если нужно

            HideUpgradePanel();
        }
    }

    private void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
        hasUpgradesAvailable = false;

        GameManager.Instance.BeginBuildPhase();
    }

    private List<UpgradeData> GetRandomUpgrades(int count)
    {
        List<UpgradeData> possibleUpgrades = new List<UpgradeData>();

        foreach (var upgrade in availableUpgrades)
        {
            switch (upgrade.type)
            {
                case UpgradeType.Equipment_Flamethrower:
                case UpgradeType.Equipment_Saw:
                case UpgradeType.Equipment_MachineGun:
                case UpgradeType.Equipment_Rocket:
                    if (!PlayerHasEquipment(upgrade.type))
                    {
                        if (Inventory.Instance.GetItems().Count < Inventory.Instance.maxSlots)
                        {
                            possibleUpgrades.Add(upgrade);
                        }
                    }
                    break;
                case UpgradeType.Upgrade_Flamethrower:
                case UpgradeType.Upgrade_Saw:
                case UpgradeType.Upgrade_MachineGun:
                case UpgradeType.Upgrade_Rocket:
                    if (PlayerHasEquipment(GetBaseEquipmentType(upgrade.type)))
                    {
                        possibleUpgrades.Add(upgrade);
                    }
                    break;
                case UpgradeType.Rail:
                    possibleUpgrades.Add(upgrade);
                    break;
            }
        }

        List<UpgradeData> finalCards = new List<UpgradeData>();
        for (int i = 0; i < count; i++)
        {
            if (possibleUpgrades.Count == 0) break;
            int randomIndex = Random.Range(0, possibleUpgrades.Count);
            finalCards.Add(possibleUpgrades[randomIndex]);
            possibleUpgrades.RemoveAt(randomIndex);
        }

        return finalCards;
    }

    private void ApplyUpgrade(UpgradeData card)
    {
        switch (card.type)
        {
            // --- FLAMETHROWER ---
            case UpgradeType.Equipment_Flamethrower:
                if (flameThrowerWeapon != null)
                {
                    flameThrowerWeapon.ActivateWeapon(card); // Activates the component
                    Inventory.Instance.AddItem(card); // Adds to inventory so we know we own it
                }
                break;
            case UpgradeType.Upgrade_Flamethrower:
                if (flameThrowerWeapon != null)
                {
                    flameThrowerWeapon.UpgradeWeapon(card); // Calls the upgrade logic
                }
                break;

            // --- SAW ---
            case UpgradeType.Equipment_Saw:
                // if (sawWeapon != null) {
                //    sawWeapon.ActivateWeapon(card);
                //    Inventory.Instance.AddItem(card);
                // }
                break;
            case UpgradeType.Upgrade_Saw:
                // if (sawWeapon != null) {
                //     sawWeapon.UpgradeWeapon(card);
                // }
                break;

            // --- MACHINE GUN ---
            case UpgradeType.Equipment_MachineGun:
                // Logic for Machine Gun activate
                break;
            case UpgradeType.Upgrade_MachineGun:
                // Logic for Machine Gun upgrade
                break;

            // --- ROCKET ---
            case UpgradeType.Equipment_Rocket:
                // Logic for Rocket activate
                break;
            case UpgradeType.Upgrade_Rocket:
                // Logic for Rocket upgrade
                break;

            // --- OTHER ---
            case UpgradeType.Rail:
                // Logic for Rail
                break;

            default:
                Debug.LogWarning($"Unknown upgrade type: {card.type}");
                break;
        }
        Debug.Log($"Applied upgrade: {card.upgradeName}");
    }

    // Вспомогательный метод для проверки наличия пушки
    private bool PlayerHasEquipment(UpgradeType baseEquipmentType)
    {
        if (Inventory.Instance == null) return false;

        // Loop through all items in our inventory
        foreach (var item in Inventory.Instance.GetItems())
        {
            // Check if any item's type matches the base equipment type we're looking for
            if (item.type == baseEquipmentType)
            {
                return true; // We found it!
            }
        }

        return false; // We don't own it
    }

    // Вспомогательный метод для получения базового типа пушки из апгрейда
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
}