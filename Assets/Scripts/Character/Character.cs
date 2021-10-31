using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, IEnemyLaserTarget
{
    [SerializeField] private Transform _laserAttackPoint;

    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private CharacterData _characterData;
    private CharacterEffects _characterEffects;
    private CharacterControl _characterControl;
    private TrolleyMovement _trolleyMovement;
    private Element _currentElement;

    private int _health;
    private int _armor;
    private int _money;
    private bool _isDeath;
    private bool _isCanReborn;

    public delegate void CharacterDeath();

    public delegate void HealthChanged(int health);

    public delegate void ArmorChanged(int armor);

    public delegate void MoneyChanged(int money);

    public delegate void ElementChanged(Element element);

    public event HealthChanged OnHealthChanged;

    public event ArmorChanged OnArmorChanged;

    public event CharacterDeath OnCharacterDeath;

    public event MoneyChanged OnMoneyChanged;

    public event ElementChanged OnElementChanged;

    public Transform CharacterTransform => transform;

    public bool IsDeath => _isDeath;

    public bool IsCanReborn => _isCanReborn;

    public int MaxHealth => _characterData.MaxHealth;

    public int MaxArmor => _characterData.MaxArmor;

    public Element CurrentElement => _currentElement;

    public Transform Transform => transform;

    public Transform LaserAttackPoint => _laserAttackPoint;

    public bool IsVisible => true;

    public void Init(CharacterData data)
    {
        GetComponents();
        _characterData = data;
        _animator.runtimeAnimatorController = _characterData.AnimatorController;
        _boxCollider2D.size = _characterData.ColliderSize;
        _boxCollider2D.offset = _characterData.ColliderOffset;
        _health = _characterData.MaxHealth;
        _armor = _characterData.MaxArmor; 
        _isDeath = false;
        _isCanReborn = true;
        SpawnStartWeapon();
    }

    public void Heal(int heal)
    {
        _health += heal;
        if (_health > _characterData.MaxHealth)
            _health = _characterData.MaxHealth;
        OnHealthChanged?.Invoke(_health);
    }

    public void Damage(int damage)
    {
        Camera.main.GetComponent<CameraManager>().ShakeCameraOnce(.1f, 6);
        if (_armor <= 0)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
                StartCoroutine(Death());
            }

            OnHealthChanged?.Invoke(_health);
        }
        else
        {
            DamageArmor(damage);
        }
    }

    public void RepairArmor(int heal)
    {
        _armor += heal;
        if (_armor > _characterData.MaxArmor)
            _armor = _characterData.MaxArmor;

        OnArmorChanged?.Invoke(_armor);
    }

    public void AddMoney(int money)
    {
        _money += money;
        OnMoneyChanged?.Invoke(_money);
    }

    public void Reborn()
    {
        _isDeath = false;
        _isCanReborn = false;
        _characterEffects.SetCharacterVisibility(true);
        _characterEffects.SpawnRebornEffect(_characterData.TeleportationAnimatorController);
        _trolleyMovement.IsMove = true;
        RepairArmor(_characterData.MaxArmor);
        Heal(_characterData.MaxHealth);
    }

    public void SetWeaponElement(Element element)
    {
        _currentElement = element;
        _characterControl.CurrentWeapon.SetElement(element.ElementType);
        OnElementChanged?.Invoke(element);
    }

    public void StartLaserInteraction()
    {
        _characterEffects.SpawnDizzinesEffect();
        _trolleyMovement.StartSpeedDebaff();
    }

    public void StopLaserInteraction()
    {
        _characterEffects.DestroyDizzinesEffect();
        _trolleyMovement.StopSpeedDebaff();
    }

    private void GetComponents()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _characterEffects = GetComponent<CharacterEffects>();
        _characterControl = GetComponent<CharacterControl>();
        _trolleyMovement = GetComponentInParent<TrolleyMovement>();
    }

    private void SpawnStartWeapon()
    {
        _currentElement = LevelSpawner.Instance.CurrentBiomeData.BiomeElement;
        _characterControl.SpawnStartWeapon(_characterData, _currentElement.ElementType);
        OnElementChanged?.Invoke(_currentElement);
    }

    private IEnumerator Death()
    {
        float hideCharacterDelay = 0.33f;
        if (!_isDeath)
        {
            _isDeath = true;
            GetComponentInParent<TrolleyMovement>().IsMove = false;
            Camera.main.GetComponent<CameraManager>().ShakeCameraOnce(.3f, 25);
            OnCharacterDeath?.Invoke();
            yield return new WaitForSeconds(hideCharacterDelay);
            _characterEffects.SetCharacterVisibility(false);
            _characterEffects.SpawnDeathEffect(_characterData.UnitColor);
        }
    }

    private void DamageArmor(int damage)
    {
        _armor -= damage;
        if (_armor < 0)
            _armor = 0;

        OnArmorChanged?.Invoke(_armor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string enemyBulletTag = "EnemyBullet";

        if (collision.CompareTag(enemyBulletTag))
        {
            BendOver(collision.transform);
            Damage(collision.GetComponent<Bullet>().Damage);
            collision.GetComponent<Bullet>().BulletHit(collision);
        }
    }

    private void BendOver(Transform hitPoint)
    {
        string turnLeftAnimatorKey = "TurnLeft";
        string turnRightAnimatorKey = "TurnRight";

        if (hitPoint.position.x >= transform.position.x)
        {
            if (transform.localScale.x == 1)
                _animator.Play(turnLeftAnimatorKey);
            else
                _animator.Play(turnRightAnimatorKey);
        }
        else if (hitPoint.position.x < transform.position.x)
        {
            if (transform.localScale.x == 1)
                _animator.Play(turnRightAnimatorKey);
            else
                _animator.Play(turnLeftAnimatorKey);
        }
    }
}