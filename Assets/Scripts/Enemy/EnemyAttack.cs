using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;

    protected BulletFactory Factory;

    private Transform target;
    private BulletData bulletData;
    private bool isAttack;
    private int damage;
    private Element.Type element;
    private AttackingEnemyData _data;

    private float timeToShoot;

    public Transform Target
    {
        set { target = value; }
    }

    public BulletData BulletData
    {
        set { bulletData = value; }
    }

    public bool IsAttack
    {
        set { isAttack = value; }
    }

    public int Damage
    {
        set { damage = value; }
    }

    public Element.Type Element
    {
        set { element = value; }
    }


    private void Start()
    {
        _data = GetComponent<Enemy>().Data as AttackingEnemyData;
        _data = _data as FlyingEnemyData;
        Factory = GetComponent<BulletFactory>();
        timeToShoot = Time.time + Random.Range(1f, 2f);
    }

    void FixedUpdate()
    {
        if (Time.time > timeToShoot && isAttack)
        {
            timeToShoot = Time.time + 1f;
            Shoot();
        }
    }

    private void Shoot()
    {
        var bullet = Factory.GetBullet(_data.BulletData.Prefab, bulletSpawnPoint);
        Quaternion dir = Quaternion.AngleAxis(0, Vector3.forward);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(dir * (GetComponent<FlyingEnemy>().Target.transform.position - bulletSpawnPoint.position + new Vector3(0, Random.Range(1, 3f), 0)).normalized * 100, ForceMode2D.Impulse);
    }
}
