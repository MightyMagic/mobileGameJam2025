using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerWeapon : WeaponBase
{
    [SerializeField] FlamethrowerTurret weaponValues;

    [SerializeField] List<WeaponValues> weaponUpgrades;

    public override void ApplyUpgradeStats(UpgradeData data)
    {
        if(data.value >= weaponUpgrades.Count)
        {
            Debug.Log("weapon is fully upgraded!");
        }
        else
        {
            for (int i = 0; i < weaponUpgrades.Count; i++)
            {
                if (weaponUpgrades[i].level == data.value)
                {
                    weaponValues.damageInterval = weaponUpgrades[i].damageInterval;
                    weaponValues.damageOverTime = weaponUpgrades[i].damageOverTime;
                }
            }

        }
       
    }
}
