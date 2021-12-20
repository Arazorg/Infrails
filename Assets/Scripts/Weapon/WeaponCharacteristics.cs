public abstract class WeaponCharacteristics
{
    private int _damage;
    private int _bulletSpeed;
    private float _fireRate;
    private float _critChance;
    private float _bulletScaleFactor;
    private float _scatter;

    public int Damage { get => _damage; set => _damage = value; }

    public int BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }

    public float FireRate { get => _fireRate; set => _fireRate = value; }

    public float CritChance { get => _critChance; set => _critChance = value; }

    public float BulletScaleFactor { get => _bulletScaleFactor; set => _bulletScaleFactor = value; }

    public float Scatter { get => _scatter; set => _scatter = value; }

    public WeaponCharacteristics(WeaponData weaponData)
    {
        _damage = weaponData.Damage;
        _bulletSpeed = weaponData.BulletSpeed;
        _fireRate = weaponData.FireRate;
        _critChance = weaponData.CritChance;
        _bulletScaleFactor = weaponData.BulletScaleFactor;
        _scatter = weaponData.Scatter;
    }

    public abstract void ImproveWeapon(int starsNumber);
}
