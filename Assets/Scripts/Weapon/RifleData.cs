using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Rifles/Standart Rifle", fileName = "New  Rifle")]
public class RifleData : WeaponData
{
    public override float GetDPS()
    {
       return (Damage + (Damage * CritChance)) / FireRate;
    }
}
