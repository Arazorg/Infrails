using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour, IEnemyLaserTarget
{
    [SerializeField] private Transform _laserAttackPoint;

    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private CharacterData _characterData;
    private CharacterEffects _characterEffects;
    private TrolleyMovement _trolleyMovement;

    private int _health;
    private int _armor;
    private int _money;
    private bool _isDeath;
    private bool _isCanReborn;

    public delegate void CharacterReborn();
    public event CharacterReborn OnCharacterReborn;

    public delegate void CharacterDeath();
    public event CharacterDeath OnCharacterDeath;

    public delegate void HealthChanged(int health, int value);
    public event HealthChanged OnHealthChanged;

    public delegate void ArmorChanged(int armor, int value);
    public event ArmorChanged OnArmorChanged;

    public delegate void MoneyChanged(int money);
    public event MoneyChanged OnMoneyChanged;

    public CharacterData CharacterData => _characterData;

    public Transform Transform => transform;

    public Transform LaserAttackPoint => _laserAttackPoint;

    public int MaxHealth => _characterData.MaxHealth;

    public int MaxArmor => _characterData.MaxArmor;

    public int Money => _money;

    public bool IsVisible => true;

    public bool IsDeath => _isDeath;

    public bool IsCanReborn => _isCanReborn;

    public void Init(CharacterData data)
    {
        SetComponents();
        _characterData = data;
        _animator.runtimeAnimatorController = _characterData.AnimatorController;
        _boxCollider2D.size = _characterData.ColliderSize;
        _boxCollider2D.offset = _characterData.ColliderOffset;
        _health = _characterData.MaxHealth;
        _armor = _characterData.MaxArmor; 
        _isDeath = false;
        _isCanReborn = true;
        GetComponentInParent<TrolleyMovement>().Character = this;
        GetComponent<CharacterWeapon>().Init(_characterData);
        GetComponent<CharacterAmplifications>().Init();
    }

    public void Heal(int heal)
    {
        _health += heal;
        if (_health > _characterData.MaxHealth)
            _health = _characterData.MaxHealth;

        OnHealthChanged?.Invoke(_health, heal);
    }

    public void Damage(int damage)
    {
        if (_armor <= 0)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
                StartCoroutine(Death());
            }

            OnHealthChanged?.Invoke(_health, -damage);
        }
        else
        {
            DamageArmor();
        }
    }

    public void RepairArmor(int heal)
    {
        _armor += heal;
        if (_armor > _characterData.MaxArmor)
            _armor = _characterData.MaxArmor;

        OnArmorChanged?.Invoke(_armor, heal);
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
        RepairArmor(_characterData.MaxArmor);
        Heal(_characterData.MaxHealth);
        OnCharacterReborn?.Invoke();
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

    private void SetComponents()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _characterEffects = GetComponent<CharacterEffects>();
        _trolleyMovement = GetComponentInParent<TrolleyMovement>();
    }

    private IEnumerator Death()
    {
        float hideCharacterDelay = 0.33f;
        if (!_isDeath)
        {
            _isDeath = true;
            OnCharacterDeath?.Invoke();
            yield return new WaitForSeconds(hideCharacterDelay);
        }
    }

    private void DamageArmor()
    {
        _armor -= 1;
        if (_armor < 0)
            _armor = 0;

        OnArmorChanged?.Invoke(_armor, -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyBullet bullet))
        {
            BendOver(collision.transform);
            Damage(bullet.Damage);
            bullet.HideBullet();
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