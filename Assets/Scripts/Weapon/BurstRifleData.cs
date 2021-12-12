using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Burst Rifles/Standart Burst Rifle", fileName = "New Burst Rifle")]
public class BurstRifleData : WeaponData
{
    [SerializeField] private int _numberOfBullets;
    [SerializeField] private float _shotsDelay;

    public int NumberOfBullets => _numberOfBullets;

    public float ShotsDelay => _shotsDelay;
}
