using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private const string PlayerBulletTag = "PlayerBullet";

    [SerializeField] protected GameObject ExplosionPrefab;
    [SerializeField] protected Transform _lighthingLaserPoint;

    protected BoxCollider2D BoxCollider2D;
    protected int Health;

    private EnemyData _data;
    private Animator _animator;
    private GameObject _target;
    private GameObject _player;
    private bool _isGetDamage;

    public EnemyData Data { get => _data; set => _data = value; }

    public GameObject Player { get => _player; set => _player = value; }

    public GameObject Target { get => _target; set => _target = value; }

    public Transform Transform => transform;

    public Transform LighthingLaserPoint => _lighthingLaserPoint;

    public bool IsGetDamage => _isGetDamage;

    public abstract void Init(EnemyData data, Transform spawnPoint, GameObject player);

    public void GetDamage(int damage)
    {
        if (_isGetDamage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
                Death();
            }
        }
    }

    public void DestroyWithoutDeathEffect()
    {
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    protected abstract void Death();

    protected void OnInit(GameObject player)
    {
        InitComponents();
        _player = player;
        _animator.runtimeAnimatorController = Data.AnimatorController;
        BoxCollider2D.size = Data.ColliderSize;
        BoxCollider2D.offset = Data.ColliderOffset;
        Health = Data.MaxHealth;
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
