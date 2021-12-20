using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Pirate Cannons/Standart Pirate Cannon", fileName = "New  Pirate Cannon")]
public class PirateCannonData : WeaponData
{
    public override float GetDPS()
    {
        return (Damage + (Damage * CritChance)) / FireRate;
    }
}
