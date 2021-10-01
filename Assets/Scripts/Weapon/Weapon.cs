using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private List<SpriteRenderer> _handsSpriteRenderer;

    protected WeaponData CurrentWeaponData;
    protected Element.Type CurrentElement;
    protected BulletFactory Factory;

    private Animator _animator;
    private BulletFactory _bulletFactory;
    private bool _isAttack;
    private float _timeToShoot = 0;

    public bool IsAttack { get => _isAttack; set => _isAttack = value; }

    public abstract void Shoot();

    public  abstract void Init(WeaponData weaponData);

    public void SetParentAndOffset(Transform parent, Vector3 offset)
    {
        transform.parent = parent;
        transform.localPosition = offset;
    }

    public void SetHands(List<Sprite> hands)
    {
        for (int i = 0; i < _handsSpriteRenderer.Count; i++)
        {
            _handsSpriteRenderer[i].sprite = hands[i];
            _handsSpriteRenderer[i].transform.localPosition = CurrentWeaponData.HandsPositions[i];
        }
    }

    public void RotateAndAttack(bool isState, float weaponAngle)
    {
        _isAttack = isState;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, weaponAngle));
    }

    protected void OnInit()
    {
        GetComponents();
        CurrentElement = GetRandomElement<Element.Type>();
        _animator.runtimeAnimatorController = CurrentWeaponData.Animator;
        _bulletSpawnPoint.localPosition = CurrentWeaponData.BulletSpawnPosition;
    }

    protected GameObject SpawnBullet()
    {
        var bullet = _bulletFactory.GetBullet(CurrentWeaponData.BulletData.Prefab, _bulletSpawnPoint);
        bullet.transform.localScale = new Vector2(CurrentWeaponData.BulletScaleFactor, CurrentWeaponData.BulletScaleFactor);
        bullet.Init(CurrentWeaponData.BulletData, CurrentWeaponData.Damage, CurrentWeaponData.CritChance, CurrentElement);
        bullet.BulletSpeed = CurrentWeaponData.BulletSpeed;
        return bullet.gameObject;
    }

    private void GetComponents()
    {
        _animator = GetComponent<Animator>();
        _bulletFactory = GetComponent<BulletFactory>();
    }

    private T GetRandomElement<T>()
    {
        var elements = Enum.GetValues(typeof(T));
        return (T)elements.GetValue(UnityEngine.Random.Range(0, elements.Length));
    }

    private void FixedUpdate()
    {
        if (Time.time > _timeToShoot && _isAttack)
        {
            _timeToShoot = Time.time + CurrentWeaponData.FireRate;
            Shoot();
        }
    }
}
