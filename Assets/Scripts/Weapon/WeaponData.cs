using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Standart Weapon", fileName = "New  Weapon")]
public class WeaponData : ItemData
{
    [Header("Weapon Resources")]
    [SerializeField] private Weapon _prefab;
    [SerializeField] private AudioClip _weaponAudioClip;
    [SerializeField] private RuntimeAnimatorController _animatorController;
    [SerializeField] private Sprite _mainSprite;
    [SerializeField] private List<Vector2> _handsPositions;

    [Header("Weapon Stats")]
    [SerializeField] private WeaponType _type;
    [SerializeField] private int _bulletSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private int _critChance;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _bulletScaleFactor;

    [Header("Bullet")]
    [SerializeField] private Vector2 _bulletSpawnPosition;
    [SerializeField] private BulletData _bulletData;

    public enum WeaponType
    {
        Shotgun,
        Rifle,
        BurstRifle
    }

    public Weapon Prefab => _prefab;

    public WeaponType Type => _type;

    public RuntimeAnimatorController Animator => _animatorController;

    public AudioClip WeaponAudioClip => _weaponAudioClip;

    public Sprite MainSprite => _mainSprite;

    public List<Vector2> HandsPositions => _handsPositions;

    public Vector2 BulletSpawnPosition => _bulletSpawnPosition;

    public BulletData BulletData => _bulletData;

    public int Damage => _damage;

    public int CritChance => _critChance;

    public float FireRate => _fireRate;

    public int BulletSpeed => _bulletSpeed;

    public float BulletScaleFactor => _bulletScaleFactor;
}
