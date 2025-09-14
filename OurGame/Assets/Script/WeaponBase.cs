using UnityEngine;

/// <summary>
/// Abstract base class for all weapons. 
/// Specific weapons (FlamethrowerWeapon, SawWeapon, etc.) should inherit from this.
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    public int currentLevel = 0;
    public int maxLevel = 8;

    /// <summary>
    /// Called when the weapon is acquired for the first time (level 0 -> 1).
    /// </summary>
    public virtual void ActivateWeapon(UpgradeData data)
    {
        currentLevel = 1;
        this.gameObject.SetActive(true); // Enable the weapon's behavior
        Debug.Log($"ACTIVATED: {this.GetType().Name} to Level {currentLevel}");
    }

    /// <summary>
    /// Called every time the weapon is upgraded (level 1 -> 2, etc.)
    /// </summary>
    public virtual void UpgradeWeapon(UpgradeData data)
    {
        if (currentLevel >= maxLevel) return;

        //currentLevel++;
        ApplyUpgradeStats(data); // Apply new stats
        Debug.Log($"UPGRADED: {this.GetType().Name} to Level {currentLevel}");
    }

    /// <summary>
    /// Override this in each weapon to define its unique level-up path.
    /// </summary>
    public abstract void ApplyUpgradeStats(UpgradeData data);
}
