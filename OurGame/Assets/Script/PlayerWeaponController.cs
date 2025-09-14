using UnityEngine;

/// <summary>
/// This script listens to the Inventory event and activates/upgrades the correct weapon
/// based on the UpgradeData's enum type.
/// 
/// ATTACH THIS TO YOUR PLAYER GAMEOBJECT.
/// </summary>
public class PlayerWeaponController : MonoBehaviour
{
    // --- WEAPON REFERENCES ---
    // You MUST drag your actual weapon components (e.g., the script component, 
    // not the GameObject) from your player/child objects into these slots in the Inspector.
    [Header("Weapon Component References")]
    public WeaponBase flamethrower; // Assign your FlamethrowerWeapon component here
    public WeaponBase saw;           // Assign your SawWeapon component here
    public WeaponBase machineGun;    // Assign your MachineGunWeapon component here
    public WeaponBase rocket;        // Assign your RocketWeapon component here


    private void OnEnable()
    {
        // Subscribe to the Inventory event
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnItemAdded += ProcessUpgradeEvent;
        }
    }

    private void OnDisable()
    {
        // ALWAYS unsubscribe to prevent errors and memory leaks
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnItemAdded -= ProcessUpgradeEvent;
        }
    }

    /// <summary>
    /// This is the listener method called by the OnItemAdded event.
    /// It uses a switch statement to route the upgrade to the correct weapon component.
    /// </summary>
    private void ProcessUpgradeEvent(UpgradeData data)
    {
        switch (data.type)
        {
            // --- FLAMETHROWER ---
            case UpgradeType.Equipment_Flamethrower:
                ActivateIfValid(flamethrower, data);
                break;
            case UpgradeType.Upgrade_Flamethrower:
                UpgradeIfValid(flamethrower, data);
                break;

            // --- SAW ---
            case UpgradeType.Equipment_Saw:
                ActivateIfValid(saw, data);
                break;
            case UpgradeType.Upgrade_Saw:
                UpgradeIfValid(saw, data);
                break;

            // --- MACHINE GUN ---
            case UpgradeType.Equipment_MachineGun:
                ActivateIfValid(machineGun, data);
                break;
            case UpgradeType.Upgrade_MachineGun:
                UpgradeIfValid(machineGun, data);
                break;

            // --- ROCKET ---
            case UpgradeType.Equipment_Rocket:
                ActivateIfValid(rocket, data);
                break;
            case UpgradeType.Upgrade_Rocket:
                UpgradeIfValid(rocket, data);
                break;

            // --- OTHER TYPES ---
            case UpgradeType.Rail:
                // Handle logic for "Rail" type
                Debug.Log("Rail upgrade applied.");
                break;

            default:
                // This will catch any enum values not handled above
                Debug.LogWarning($"Unhandled UpgradeType: {data.type}");
                break;
        }
    }

    /// <summary>
    /// Helper function to safely activate a weapon.
    /// </summary>
    private void ActivateIfValid(WeaponBase weapon, UpgradeData data)
    {
        // Check if the weapon reference is set AND the weapon has not been acquired yet
        if (weapon != null && weapon.currentLevel == 0)
        {
            weapon.ActivateWeapon(data);
        }
    }

    /// <summary>
    /// Helper function to safely upgrade an existing weapon.
    /// </summary>
    private void UpgradeIfValid(WeaponBase weapon, UpgradeData data)
    {
        // Check if the weapon reference is set AND the weapon is already active (level > 0)
        if (weapon != null && weapon.currentLevel > 0)
        {
            weapon.UpgradeWeapon(data);
        }
    }
}
