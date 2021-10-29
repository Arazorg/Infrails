using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private List<SpriteRenderer> _handsSpriteRenderer;

    protected WeaponData CurrentWeaponData;
    protected BulletFactory Factory;
    protected MonobehaviourPool<Bullet> _bulletsPool;

    private Animator _animator;
    private BulletFactory _bulletFactory;
    private Element.Type _currentElement;
    private bool _isAttack;
    private float _timeToShoot = 0;

    public bool IsAttack { get => _isAttack; set => _isAttack = value; }

    public Element.Type CurrentElement { get => _currentElement; set => _currentElement = value; }

    public abstract void Shoot();

    public abstract void Init(WeaponData weaponData);

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

    public void Rotate(float weaponAngle)
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, weaponAngle));
    }

    public void SetElement(Element.Type element)
    {
        CurrentElement = element;
    }

    protected void OnInit()
    {
        GetComponents();
        _animator.runtimeAnimatorController = CurrentWeaponData.Animator;
        _bulletSpawnPoint.localPosition = CurrentWeaponData.BulletSpawnPosition;
        _bulletsPool = new MonobehaviourPool<Bullet>(0, true, _bulletFactory, CurrentWeaponData.BulletData.Prefab, _bulletSpawnPoint);
    }

    protected GameObject SpawnBullet()
    {
        var bullet = _bulletsPool.GetFreeElement();
        bullet.transform.position = _bulletSpawnPoint.position;
        bullet.Init(CurrentWeaponData.BulletData, CurrentWeaponData, CurrentElement);
        return bullet.gameObject;
    }

    private void GetComponents()
    {
        _animator = GetComponent<Animator>();
        _bulletFactory = GetComponent<BulletFactory>();
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
