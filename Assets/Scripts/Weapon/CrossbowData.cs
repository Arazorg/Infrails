using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Crossbows/Standart Crossbow", fileName = "New  Crossbow")]
public class CrossbowData : WeaponData
{
    public override float GetDPS()
    {
        return (Damage + (Damage * CritChance)) / FireRate;
    }
}
