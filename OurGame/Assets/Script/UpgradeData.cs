using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/Upgrade Card")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea(3, 5)]
    public string description;
    public int cost;

    [Header("Icons")]
    public Sprite cardIcon; // »конка дл€ отображени€ на карте выбора
    public Sprite inventoryIcon; // »конка дл€ отображени€ в инвентаре

    public UpgradeType type;
    public float value;

}