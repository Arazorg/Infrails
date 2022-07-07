using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _centerPoint;

    protected BoxCollider2D BoxCollider2D;
    protected Character Character;

    private EnemyData _data;
    private Animator _animator;
    private int _health;
    private bool _isGetDamage;
    private bool _isDeath;

    public delegate void EnemyDeath();

    public event EnemyDeath OnEnemyDeath;

    public delegate void EnemyDamage(int damage);

    public event EnemyDamage OnEnemyDamage;

    public EnemyData Data { get => _data; set => _data = value; }

    public Transform Transform => transform;

    public bool IsGetDamage { get => _isGetDamage; set => _isGetDamage = value; }

    public int Health { get => _health; set => _health = value; }

    public Transform CenterPoint => _centerPoint;

    public abstract void Init(EnemyData data, Transform spawnPoint, Character character);

    public abstract void BulletHit(PlayerBullet bullet);

    public void GetDamage(int damage)
    {
        if (_isGetDamage)
        {
            _health -= damage;
            OnEnemyDamage?.Invoke(damage);
            if (_health <= 0)
            {
                _health = 0;
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
        BoxCollider2D.enabled = false;
        _centerPoint.localPosition = Data.Center;
        _health = Data.MaxHealth;
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
        if (collision.TryGetComponent(out PlayerBullet bullet))
            BulletHit(bullet);
    }

    private void OnBecameVisible()
    {
        BoxCollider2D.enabled = true;
        _isGetDamage = true;
    }

    private void OnBecameInvisible()
    {
        BoxCollider2D.enabled = false;
        _isGetDamage = false;
    }
}
