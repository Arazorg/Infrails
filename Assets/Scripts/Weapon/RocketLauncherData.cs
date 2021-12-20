using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Rocket Launchers/Standart Rocket Launchers", fileName = "New  Rocket Launcher")]

public class RocketLauncherData : WeaponData
{
    [SerializeField] private float _explosionRadius;

    public float ExplosionRadius => _explosionRadius;

    public override float GetDPS()
    {
        return (Damage + (Damage * CritChance)) / FireRate;
    }
}
