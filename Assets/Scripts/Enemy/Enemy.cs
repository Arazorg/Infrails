using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IArrowTarget
{
    [SerializeField] protected GameObject ExplosionPrefab;
    [SerializeField] private GameObject _damageEffect;
    [SerializeField] private Transform _arrowSpawnPoint;
    [SerializeField] private PopUpDamageText _popUpText;

    protected BoxCollider2D BoxCollider2D;
    protected Character Character;
    protected int Health;

    private Coroutine _bleedingCoroutine;
    private EnemyData _data;
    private Animator _animator;
    private bool _isGetDamage;
    private bool _isDeath;
    private float bleedingFinishTime;

    public EnemyData Data { get => _data; set => _data = value; }

    public Transform Transform => transform;

    public bool IsGetDamage { get => _isGetDamage; set => _isGetDamage = value; }

    public Transform ArrowSpawnPoint => _arrowSpawnPoint;

    public abstract void Init(EnemyData data, Transform spawnPoint, Character character);

    public void GetDamage(int damage)
    {
        if (_isGetDamage)
        {
            Health -= damage;
            _popUpText.ShowPopUpText(damage, Data.PopUpTextOffset);
            var particleSettings = _damageEffect.GetComponent<ParticleSystem>().main;
            particleSettings.startColor = Data.UnitColor;
            GameObject explosion = Instantiate(_damageEffect, transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
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
        _arrowSpawnPoint.localPosition = Data.Center;
        _damageEffect.transform.localPosition = Data.Center;
        _isDeath = false;
    }

    protected void SpawnExplosionParticle()
    {
        SetParticleColor(Data.UnitColor);
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position + (Vector3)Data.Center, Quaternion.identity);
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
        if (collision.TryGetComponent(out Bullet bullet))
        {
            if (!(bullet is EnemyBullet))
            {
                GetDamage(bullet.Damage);
                bullet.BulletHit(transform);
            }
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

    public void ArrowHitEffect()
    {
        float bleedingDuration = 3f;
        if (_bleedingCoroutine == null)
        {
            bleedingFinishTime = Time.time + bleedingDuration;
            _bleedingCoroutine = StartCoroutine(Bleeding());
        }
        else
        {
            bleedingFinishTime = Time.time + bleedingDuration;
        }
    }

    private IEnumerator Bleeding()
    {
        while (Time.time < bleedingFinishTime)
        {
            int bleedingDamage = 1;
            float timeBetweenDamage = 0.75f;
            yield return new WaitForSeconds(timeBetweenDamage);
            GetDamage(bleedingDamage);
        }
    }
}
