// PlayerEquipmentManager.cs
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    // Ссылки на все префабы оружия, которые вы хотите активировать/деактивировать
    // Эти объекты должны быть дочерними к персонажу и изначально неактивными
    public GameObject flamethrowerPrefab;
    public GameObject sawPrefab;
    public GameObject machineGunPrefab;

    // Вспомогательный метод для активации нужного оружия
    public void EquipWeapon(UpgradeType type)
    {
        // Деактивируем все оружия, чтобы не было конфликта
        flamethrowerPrefab.SetActive(false);
        sawPrefab.SetActive(false);
        machineGunPrefab.SetActive(false);

        // Активируем нужный префаб на основе типа апгрейда
        switch (type)
        {
            case UpgradeType.Equipment_Flamethrower:
                flamethrowerPrefab.SetActive(true);
                break;
            case UpgradeType.Equipment_Saw:
                sawPrefab.SetActive(true);
                break;
            case UpgradeType.Equipment_MachineGun:
                machineGunPrefab.SetActive(true);
                break;
        }
    }
}