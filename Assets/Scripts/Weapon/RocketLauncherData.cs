using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Rocket Launchers/Standart Rocket Launchers", fileName = "New  Rocket Launcher")]

public class RocketLauncherData : WeaponData
{
    [SerializeField] private float _explosionRadius;

    public float ExplosionRadius => _explosionRadius;

    /*public override void ImproveWeapon(int starsNumber)
    {
        Stars = starsNumber;

        float explosionRadiusForLevel = 0.33f;
        float damageForLevel = 0.5f;
        float bulletSpeedForLevel = 5f;

        _explosionRadius += explosionRadiusForLevel * StarsNumber;
        Damage += (int)(damageForLevel * StarsNumber);
        BulletSpeed += (int)(bulletSpeedForLevel * StarsNumber);
    }*/
}
