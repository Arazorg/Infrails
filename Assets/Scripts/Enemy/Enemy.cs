using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private const string PlayerBulletTag = "PlayerBullet";

    [SerializeField] protected GameObject ExplosionPrefab;

    protected BoxCollider2D BoxCollider2D;
    protected Character Character;
    protected int Health;

    private EnemyData _data;
    private Animator _animator;
    
    private bool _isGetDamage;
    private bool _isDeath;

    public EnemyData Data { get => _data; set => _data = value; }

    public Transform Transform => transform;

    public bool IsGetDamage { get => _isGetDamage; set => _isGetDamage = value; }

    public abstract void Init(EnemyData data, Transform spawnPoint, Character character);

    public void GetDamage(int damage)
    {
        if (_isGetDamage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
                if (!_isDeath)
                {
                    _isDeath = true;
                    Death(GameConstants.DeathWithEffect);
                }                   
            }
        }
    }

    protected abstract void Death(bool isDeathWithEffect = false);

    protected void OnInit()
    {
        InitComponents();
        _animator.runtimeAnimatorController = Data.AnimatorController;
        BoxCollider2D.size = Data.ColliderSize;
        BoxCollider2D.offset = Data.ColliderOffset;
        Health = Data.MaxHealth;
        _isDeath = false;
    }

    protected void SpawnExplosionParticle()
    {
        SetParticleColor(Data.UnitColor);
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }

    private void InitComponents()
    {
        _animator = GetComponent<Animator>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void SetParticleColor(Color color)
    {
        var particleSettings = ExplosionPrefab.GetComponent<ParticleSystem>().main;
        particleSettings.startColor = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(PlayerBulletTag))
        {
            Bullet bullet = collision.transform.GetComponent<Bullet>();
            GetDamage(bullet.Damage);
            bullet.BulletHit(collision);
        }
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
