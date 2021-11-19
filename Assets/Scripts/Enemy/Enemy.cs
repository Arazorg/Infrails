using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected BoxCollider2D BoxCollider2D;
    protected Character Character;
    protected int Health;

    private EnemyData _data;
    private Animator _animator;
    private bool _isGetDamage;
    private bool _isDeath;

    public delegate void EnemyDeath();

    public event EnemyDeath OnEnemyDeath;

    public delegate void EnemyDamage(int damage);

    public event EnemyDamage OnEnemyDamage;

    public EnemyData Data { get => _data; set => _data = value; }

    public Transform Transform => transform;

    public bool IsGetDamage { get => _isGetDamage; set => _isGetDamage = value; }

    public abstract void Init(EnemyData data, Transform spawnPoint, Character character);

    public abstract void BulletHit(Bullet bullet);

    public void GetDamage(int damage)
    {
        if (_isGetDamage)
        {
            Health -= damage;
            OnEnemyDamage?.Invoke(damage);
            if (Health < 0)
            {
                Health = 0;
                if (!_isDeath)
                {
                    _isDeath = true;
                    OnEnemyDeath?.Invoke();
                    Death(GameConstants.DeathWithEffect);
                }
            }
        }
    }

    protected abstract void Death(bool isDeathWithEffect = false);

    protected void OnInit()
    {
        InitComponents();
        BoxCollider2D.size = Data.ColliderSize;
        BoxCollider2D.offset = Data.ColliderOffset;
        Health = Data.MaxHealth;
        _animator.runtimeAnimatorController = Data.AnimatorController;
        _isDeath = false;
    }

    private void InitComponents()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bullet bullet))
            BulletHit(bullet);
    }

    private void OnBecameVisible()
    {
        _isGetDamage = true;
    }

    private void OnBecameInvisible()
    {
        _isGetDamage = false;
    }
}
