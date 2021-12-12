using UnityEngine;

public class EnemyWeaponCharacteristics : WeaponCharacteristics
{
    public EnemyWeaponCharacteristics(WeaponData weaponData) : base(weaponData)
    {
        ImproveWeapon(CurrentGameInfo.Instance.ReachedBiomeNumber);
    }

    public override void ImproveWeapon(int starsNumber)
    {
        float damageForBiome = 0.09f;
        float fireRateForBiome = -0.01f;
        FireRate += fireRateForBiome * starsNumber;
        Damage += (int)(damageForBiome * starsNumber);
    }
}
