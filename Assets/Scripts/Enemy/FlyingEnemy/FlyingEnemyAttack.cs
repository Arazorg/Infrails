using System.Collections;
using UnityEngine;

public class FlyingEnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPoint;

    private BulletFactory _bulletFactory;
    private MonobehaviourPool<Bullet> _bulletsPool;
    private Transform _target;
    private AttackingEnemyData _data;
    private float _needAngleZ;
    private float _stopAttackTime;
    private bool _isFacingRight;


    public delegate void AttackFinished();

    public event AttackFinished OnAttackFinished;

    public void Init(Transform target, EnemyData data)
    {
        _target = target;
        _data = data as AttackingEnemyData;
        GetComponent<SpriteRenderer>().flipX = data.IsFlipX;
        _bulletSpawnPoint.localPosition = _data.WeaponData.BulletSpawnPosition;
        _bulletFactory = GetComponent<BulletFactory>();
        _bulletsPool = new MonobehaviourPool<Bullet>(0, true, _bulletFactory, _data.WeaponData.BulletData.Prefab, _bulletSpawnPoint);
    }

    public void StartAttack()
    {
        float attackDuration = 1.5f;
        _stopAttackTime = Time.time + attackDuration;
        _needAngleZ = GetNeedAngle();
        StartCoroutine(Attacking());
    }

    public void StopAttack()
    {
        StopCoroutine(Attacking());
    }

    public void DestroyPoolBullets()
    {
        foreach (var bullet in _bulletsPool.Pool)
            bullet.DestroyBullet();
    }

    private IEnumerator Attacking()
    {
        while (true)
        {
            if (Time.time > _stopAttackTime)
                break;

            yield return new WaitForSeconds(_data.WeaponData.FireRate);
            Shoot();
        }

        _needAngleZ = 0;
        OnAttackFinished?.Invoke();
    }

    private void Shoot()
    {
        AudioManager.Instance.PlayEffect(_data.WeaponData.WeaponAudioClip);
        var bullet = _bulletsPool.GetFreeElement();
        bullet.transform.position = _bulletSpawnPoint.position;
        bullet.Init(_data.WeaponData, Element.Type.None);
        Quaternion dir = Quaternion.AngleAxis(0, Vector3.forward);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        Vector3 vectorToTarget = transform.right.normalized * GetFacingDirectionFactor();
        bulletRb.AddForce(dir * vectorToTarget * _data.WeaponData.BulletSpeed, ForceMode2D.Impulse);
    }

    private float GetNeedAngle()
    {
        float minAngle = 12.5f;
        float maxAngle = 17.5f;
        float angleZ;
        if (transform.position.y > _target.position.y)
            angleZ = Random.Range(minAngle, maxAngle);
        else
            angleZ = Random.Range(-maxAngle, minAngle);

        return angleZ;
    }

    private void Update()
    {
        TurnToTarget();
        RotateToTarget();
    }

    private void TurnToTarget()
    {
        if (_target != null)
        {
            if (_target.position.x < transform.position.x && _isFacingRight)
                Flip();
            else if (_target.position.x > transform.position.x && !_isFacingRight)
                Flip();
        }
    }

    private void RotateToTarget()
    {
        float rotationSpeed = 5;
        Quaternion _needQuaternion = Quaternion.Euler(new Vector3(0, 0, _needAngleZ));
        transform.rotation = Quaternion.Lerp(transform.rotation, _needQuaternion, Time.deltaTime * rotationSpeed);
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;   
    }

    private float GetFacingDirectionFactor()
    {
        if (_isFacingRight)
            return 1;
        else
            return -1;
    }
}