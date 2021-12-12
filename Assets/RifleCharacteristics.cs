public class RifleCharacteristics : WeaponCharacteristics
{
    public RifleCharacteristics(WeaponData weaponData) : base(weaponData)
    {
        ImproveWeapon(weaponData.StarsNumber);
    }

    public override void ImproveWeapon(int starsNumber)
    {
        float critChanceForStar = 0.01f;
        float damageForStar = 0.4f;
        float bulletSpeedForStar = 6f;

        Damage += (int)(damageForStar * starsNumber);
        CritChance += critChanceForStar * starsNumber;
        BulletSpeed += (int)(bulletSpeedForStar * starsNumber);
    }
}
