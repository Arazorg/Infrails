using System.Collections;
using UnityEngine;

public class FlyingEnemyAttack : MonoBehaviour
{
    private const float EnemyBulletSpeed = 90;

    [SerializeField] private Transform bulletSpawnPoint;

    protected BulletFactory _bulletFactory;
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
        _isFacingRight = data.IsSpriteFacingRight;
        _bulletFactory = GetComponent<BulletFactory>();
        bulletSpawnPoint.localPosition = (_data as FlyingEnemyData).BulletSpawnPosition;
    }

    public void StartAttack()
    {
        float attackDuration = 1f;
        _stopAttackTime = Time.time + attackDuration;
        _needAngleZ = GetNeedAngle();
        StartCoroutine(Attacking());
    }

    public void StopAttack()
    {
        StopCoroutine(Attacking());
    }

    private IEnumerator Attacking()
    {        
        while (true)
        {
            if (Time.time > _stopAttackTime)
                break;

            yield return new WaitForSeconds(_data.FireRate);
            Shoot();
        }

        _needAngleZ = 0;
        OnAttackFinished?.Invoke();
    }

    private void Shoot()
    {
        var bullet = _bulletFactory.GetBullet(_data.BulletData.Prefab, bulletSpawnPoint);
        bullet.Init(_data.BulletData, _data);
        Quaternion dir = Quaternion.AngleAxis(0, Vector3.forward);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        Vector3 vectorToTarget = (transform.right * -GetFacingDirectionFactor()).normalized;
        bulletRb.AddForce(dir * vectorToTarget * EnemyBulletSpeed, ForceMode2D.Impulse);
    }

    private float GetNeedAngle()
    {
        float angleZ;
        if (transform.position.y > _target.position.y)
            angleZ = Random.Range(10, 15);
        else
            angleZ = Random.Range(-15, 10);

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
        Quaternion _needQuaternion = Quaternion.Euler(new Vector3(0, 0, _needAngleZ * GetFacingDirectionFactor()));
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
            return -1;
        else
            return 1;
    }
}
