// PlayerEquipmentManager.cs
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    // Ссылки на все префабы оружия, которые вы хотите активировать/деактивировать
    // Эти объекты должны быть дочерними к персонажу и изначально неактивными
    public GameObject flamethrowerPrefab;
    public GameObject sawPrefab;
    public GameObject machineGunPrefab;
    public GameObject rocketPrefab;

    // Вспомогательный метод для активации нужного оружия
    public void EquipWeapon(UpgradeType type)
    {
        // Деактивируем все оружия, чтобы не было конфликта
        if (flamethrowerPrefab != null) flamethrowerPrefab.SetActive(false);
        if (sawPrefab != null) sawPrefab.SetActive(false);
        if (machineGunPrefab != null) machineGunPrefab.SetActive(false);
        if (rocketPrefab != null) rocketPrefab.SetActive(false);

        // Активируем нужный префаб на основе типа апгрейда
        switch (type)
        {
            case UpgradeType.Equipment_Flamethrower:
                if (flamethrowerPrefab != null) flamethrowerPrefab.SetActive(true);
                break;
            case UpgradeType.Equipment_Saw:
                if (sawPrefab != null) sawPrefab.SetActive(true);
                break;
            case UpgradeType.Equipment_MachineGun:
                if (machineGunPrefab != null) machineGunPrefab.SetActive(true);
                break;
            case UpgradeType.Equipment_Rocket:
                if (rocketPrefab != null) rocketPrefab.SetActive(true);
                break;
        }
    }
}