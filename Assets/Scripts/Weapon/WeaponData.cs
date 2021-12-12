using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData : ItemData
{
    [Header("Weapon Resources")]
    [SerializeField] private Weapon _prefab;
    [SerializeField] private AudioClip _weaponAudioClip;
    [SerializeField] private RuntimeAnimatorController _animatorController;
    [SerializeField] private Sprite _mainSprite;
    [SerializeField] private List<Vector2> _handsPositions;

    [Header("Weapon Stats")]
    [SerializeField] private int _bulletSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private float _critChance;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _bulletScaleFactor;
    [SerializeField] private float _scatter;
    [SerializeField] private int _level;

    [Header("Bullet")]
    [SerializeField] private Vector2 _bulletSpawnPosition;
    [SerializeField] private BulletData _bulletData;

    private int _starsNumber = 1;

    public Weapon Prefab => _prefab;

    public RuntimeAnimatorController Animator => _animatorController;

    public AudioClip WeaponAudioClip => _weaponAudioClip;

    public Sprite MainSprite => _mainSprite;

    public List<Vector2> HandsPositions => _handsPositions;

    public Vector2 BulletSpawnPosition => _bulletSpawnPosition;

    public BulletData BulletData => _bulletData;

    public int StarsNumber { get => _starsNumber; set => _starsNumber = value; }

    public int Damage => _damage;

    public float CritChance => _critChance;

    public float FireRate => _fireRate;

    public int BulletSpeed => _bulletSpeed;

    public float BulletScaleFactor => _bulletScaleFactor;

    public float Scatter => _scatter;

    public int Level => _level;
}
