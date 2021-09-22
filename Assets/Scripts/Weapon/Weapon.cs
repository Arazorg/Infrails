using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponData CurrentWeaponData;
    protected ElementsResistance.Elements CurrentElement;
    protected BulletSpawner BulletSpawner;

    private Animator _animator;
    private bool _isAttack;
    private float _timeToShoot = 0;

    public bool IsAttack => _isAttack;

    public abstract void Shoot();

    public void Init(WeaponData data)
    {
        GetComponents();
        CurrentWeaponData = data;
        CurrentElement = GetRandomElement<ElementsResistance.Elements>();
        BulletSpawner.BulletSpawnPosition = data.BulletSpawnPosition;
        _animator.runtimeAnimatorController = data.Animator;
    }

    public void SetParentAndOffset(Transform parent, Vector3 offset)
    {
        transform.parent = parent;
        transform.localPosition = offset;
    }

    public void SetHands(List<Sprite> hands)
    {
        for (int i = 1; i < 3; i++)
        {
            var hand = transform.GetChild(i);
            hand.GetComponent<SpriteRenderer>().sprite = hands[i - 1];
            switch (i)
            {
                case 1:
                    hand.localPosition = CurrentWeaponData.FirstHandPosition;
                    break;
                case 2:
                    hand.localPosition = CurrentWeaponData.SecondHandPosition;
                    break;
            }
        }
    }

    public void RotateAndAttack(bool isState, float weaponAngle)
    {
        _isAttack = isState;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, weaponAngle));
    }

    private void GetComponents()
    {
        _animator = GetComponent<Animator>();
        BulletSpawner = GetComponent<BulletSpawner>();
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
            //Shoot();
        }
    }
}
