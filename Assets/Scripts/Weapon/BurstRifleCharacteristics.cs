public class BurstRifleCharacteristics : WeaponCharacteristics
{
    private int _numberOfBullets;
    private float _shotsDelay;

    public int NumberOfBullets { get => _numberOfBullets; set => _numberOfBullets = value; }

    public float ShotsDelay { get => _shotsDelay; set => _shotsDelay = value; }

    public BurstRifleCharacteristics(BurstRifleData burstRifleData) : base(burstRifleData)
    {
        _numberOfBullets = burstRifleData.NumberOfBullets;
        _shotsDelay = burstRifleData.ShotsDelay;
        ImproveWeapon(burstRifleData.StarsNumber);
    }

    public override void ImproveWeapon(int starsNumber)
    {
        float shotDelayForStar = -0.02f;
        float fireRateForStar = -0.02f;
        float numberOfBulletsForStar = 0.25f;
        float damageForStar = 0.5f;
        Damage += (int)(damageForStar * starsNumber);
        FireRate += (fireRateForStar * starsNumber);
        _shotsDelay += shotDelayForStar * starsNumber;
        _numberOfBullets += (int)(numberOfBulletsForStar * starsNumber);
    }
}
