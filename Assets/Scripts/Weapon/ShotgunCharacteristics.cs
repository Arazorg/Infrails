public class ShotgunCharacteristics : WeaponCharacteristics
{
    private int _minNumberOfBullets;
    private int _maxNumberOfBullets;
    private float _bulletSpeedSpread;

    public int MinNumberOfBullets { get => _minNumberOfBullets; set => _minNumberOfBullets = value; }

    public int MaxNumberOfBullets { get => _maxNumberOfBullets; set => _maxNumberOfBullets = value; }

    public float BulletSpeedSpread { get => _bulletSpeedSpread; set => _bulletSpeedSpread = value; }

    public ShotgunCharacteristics(ShotgunData shotgunData) : base(shotgunData)
    {
        _minNumberOfBullets = shotgunData.MinNumberOfBullets;
        _maxNumberOfBullets = shotgunData.MaxNumberOfBullets;
        _bulletSpeedSpread = shotgunData.BulletSpeedSpread;
        ImproveWeapon(shotgunData.StarsNumber);
    }

    public override void ImproveWeapon(int starsNumber)
    {
        float numberOfBulletsForStar = 0.6f;
        float scatterForStar = -1f;
        float fireRateForStar = -0.05f;
        float bulletSpreadSpeedForStar = -0.01f;

        _minNumberOfBullets += (int)(numberOfBulletsForStar * starsNumber);
        _maxNumberOfBullets += (int)(numberOfBulletsForStar * starsNumber);
        _bulletSpeedSpread += bulletSpreadSpeedForStar * starsNumber;
        Scatter += scatterForStar * starsNumber;
        FireRate += fireRateForStar * starsNumber;
    }
}
