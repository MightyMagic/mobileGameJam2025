using UnityEngine;
// Добавьте необходимые using-директивы

public class SawWeapon : WeaponBase
{
    // Добавьте поля и свойства для ракетницы, например, скорость, урон и т.д.

    public override void ActivateWeapon(UpgradeData data)
    {
        // Здесь будет логика для активации ракетницы
        Debug.Log("SawWeapon activated!");
        // Например, включаем модель оружия, систему частиц и т.д.
    }

    public override void ApplyUpgradeStats(UpgradeData data)
    {
        // Здесь будет логика для апгрейда характеристик ракетницы
        Debug.Log("SawWeapon upgraded!");
    }
}