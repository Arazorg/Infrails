using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Shotguns/Standart Shotgun", fileName = "New  Shotgun")]
public class ShotgunData : WeaponData
{
    [SerializeField] private int _minNumberOfBullets;
    [SerializeField] private int _maxNumberOfBullets;
    [SerializeField] private float _bulletSpeedSpread;

    public int MinNumberOfBullets => _minNumberOfBullets;

    public int MaxNumberOfBullets => _maxNumberOfBullets;

    public float BulletSpeedSpread => _bulletSpeedSpread;
}
