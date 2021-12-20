using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Standart Enemy Weapon", fileName = "New  Enemy Weapon")]
public class EnemyWeaponData : WeaponData
{
    public override float GetDPS()
    {
        return Damage / FireRate;
    }
}
