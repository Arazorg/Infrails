public class RifleCharacteristics : WeaponCharacteristics
{
    public RifleCharacteristics(WeaponData weaponData) : base(weaponData)
    {
        ImproveWeapon(weaponData.StarsNumber);
    }

    public override void ImproveWeapon(int starsNumber)
    {
        float critChanceForStar = 0.02f;
        float damageForStar = 1f;
        float fireRateForStar = -0.05f;
        float bulletSpeedForStar = 5f;

        Damage += (int)(damageForStar * starsNumber);

        float minFireRate = 0.1f;
        FireRate += fireRateForStar * starsNumber;
        if (FireRate < minFireRate)
            FireRate = minFireRate;

        CritChance += critChanceForStar * starsNumber;
        BulletSpeed += (int)(bulletSpeedForStar * starsNumber);
    }
}
