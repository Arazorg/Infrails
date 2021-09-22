using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider2D _boxCollider2D;
    private CharacterData _characterData;
    private CharacterEffects _characterEffects;

    private int _health;
    private int _armor;
    private int _money;
    private bool _isDeath;
    private bool _isCanReborn;

    public delegate void CharacterDeath();

    public delegate void HealthChanged(int health);

    public delegate void ArmorChanged(int armor);

    public delegate void MoneyChanged(int money);

    public event HealthChanged OnHealthChanged;

    public event ArmorChanged OnArmorChanged;

    public event CharacterDeath OnCharacterDeath;

    public event MoneyChanged OnMoneyChanged;

    public bool IsCanReborn => _isCanReborn;

    public int MaxHealth => _characterData.MaxHealth;

    public int MaxArmor => _characterData.MaxArmor;

    public void Init(CharacterData data)
    {
        GetComponents();
        _characterData = data;
        _animator.runtimeAnimatorController = _characterData.AnimatorController;
        _boxCollider2D.size = _characterData.ColliderSize;
        _boxCollider2D.offset = _characterData.ColliderSize;
        _health = _characterData.MaxHealth;
        _armor = _characterData.MaxArmor;
        _isDeath = false;
        _isCanReborn = true;
        GetComponent<CharacterControl>().SpawnWeapon(_characterData, data.WeaponSpawnPoint);
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

    public void HealArmor(int heal)
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
        GetComponentInParent<TrolleyMovement>().IsMove = true;
        HealArmor(_characterData.MaxArmor);
        Heal(_characterData.MaxHealth);
    }

    private void GetComponents()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _characterEffects = GetComponent<CharacterEffects>();
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
        string turnLeftAnimatorKey = "TurnLeft";
        string turnRightAnimatorKey = "TurnRight";
        string enemyBulletTag = "EnemyBullet";

        if (collision.CompareTag(enemyBulletTag))
        {
            if (collision.transform.position.x >= transform.position.x)
            {
                if (transform.localScale.x == 1)
                    _animator.Play(turnLeftAnimatorKey);
                else
                    _animator.Play(turnRightAnimatorKey);
            }
            else if (collision.transform.position.x < transform.position.x)
            {
                if (transform.localScale.x == 1)
                    _animator.Play(turnRightAnimatorKey);
                else
                    _animator.Play(turnLeftAnimatorKey);
            }

            // Damage(collision.GetComponent<Bullet>().Damage);
            // collision.GetComponent<Bullet>().DestroyBullet();
        }
    }
}