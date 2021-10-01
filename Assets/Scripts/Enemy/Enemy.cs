using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private const string PlayerBulletTag = "PlayerBullet";

    protected BoxCollider2D boxCollider2D;
    protected EnemyData data;
    protected bool isDeath;
    protected Character characterScript;
    protected int _health;

    private Animator animator;
    private bool _isGetDamage;

    public void Init(EnemyData data, GameObject character =  null, Transform spawnPoint = null)
    {
        InitComponents();
        animator.runtimeAnimatorController = data.AnimatorController;
        boxCollider2D.size = data.ColliderSize;
        boxCollider2D.offset = data.ColliderOffset;
        _health = data.MaxHealth;
    }

    public void GetDamage(int damage)
    {
        if (_isGetDamage)
        {
            _health -= damage;
            if (_health < 0)
            {
                _health = 0;
                Death();
            }
        }
    }

    public abstract void Death();

    private void InitComponents()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnBecameVisible()
    {
        _isGetDamage = true;
    }

    private void OnBecameInvisible()
    {
        _isGetDamage = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(PlayerBulletTag))
        {
            Bullet bullet = collision.transform.GetComponent<Bullet>();
            if (Random.Range(0, 1f) > bullet.CritChance)
                GetDamage(bullet.Damage * 2);
            else
                GetDamage(bullet.Damage);

            bullet.BulletHit(collision);
        }
    }
}
