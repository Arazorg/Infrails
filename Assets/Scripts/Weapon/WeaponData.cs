using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Standart Weapon", fileName = "New  Weapon")]
public class WeaponData : ItemData
{
    [SerializeField] private Weapon _prefab;
    [SerializeField] private WeaponType _type;
    [SerializeField] private RuntimeAnimatorController _animatorController;
    [SerializeField] private Sprite _mainSprite;
    [SerializeField] private Vector2 _firstHandPosition;
    [SerializeField] private Vector2 _secondHandPosition;
    [SerializeField] private Vector2 _bulletSpawnPosition;
    [SerializeField] private BulletData _bulletData;
    [SerializeField] private int _bulletSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private int _critChance;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _bulletScaleFactor;

    public enum WeaponType
    {
        Shotgun,
        Rifle,
        Laser,
        BurstRifle
    }

    public Weapon Prefab => _prefab;

    public WeaponType Type => _type;

    public RuntimeAnimatorController Animator => _animatorController;

    public Sprite MainSprite => _mainSprite;

    public Vector2 FirstHandPosition => _firstHandPosition;

    public Vector2 SecondHandPosition => _secondHandPosition;

    public Vector2 BulletSpawnPosition => _bulletSpawnPosition;

    public BulletData BulletData => _bulletData;

    public int Damage => _damage;

    public int CritChance => _critChance;

    public float FireRate => _fireRate;

    public int BulletSpeed => _bulletSpeed;

    public float BulletScaleFactor => _bulletScaleFactor;
}
