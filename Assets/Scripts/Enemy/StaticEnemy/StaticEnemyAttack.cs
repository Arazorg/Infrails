using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyAttack : MonoBehaviour
{
    [SerializeField] private StaticEnemyWeapon _weapon;

    private StaticEnemyData _staticEnemyData;
    private List<IEnemyLaserTarget> _availableTargets;
    private IEnemyLaserTarget _currentTarget;
    private IEnemyLaserTarget _character;
    private bool _isFacingRight;

    public delegate void TargetBecameNull();

    public event TargetBecameNull OnTargetBecameNull;

    public IEnemyLaserTarget Character { set => _character = value; }

    public void Init(StaticEnemyData staticEnemyData, List<IEnemyLaserTarget> targets)
    {
        _staticEnemyData = staticEnemyData;
        _availableTargets = targets;
        InitWeapon();
        _isFacingRight = true;
    }

    public void StartAttack()
    {
        SetTarget();
    }

    public void StopAttack()
    {
        _weapon.DestroyLaser();
    }

    private void InitWeapon()
    {
        _weapon.transform.localPosition = _staticEnemyData.WeaponSpawnPosition;
        _weapon.InitWeapon(_staticEnemyData);
    }

    private void SetTarget()
    {
        _availableTargets.RemoveAll(s => s == null);
        _currentTarget = GetNearestTarget();
        if (_currentTarget == null)
            _currentTarget = _character;

        _weapon.SetTarget(_currentTarget);
    }

    private IEnemyLaserTarget GetNearestTarget()
    {
        float smallestDistance = float.MaxValue;
        float minDistance = 15f;
        float maxDistance = 45f;

        IEnemyLaserTarget target = null;
        foreach (var availableTarget in _availableTargets.FindAll(s => s.IsVisible))
        {
            float distance = Vector2.Distance(availableTarget.LaserAttackPoint.position, transform.position);
            if (distance < smallestDistance && distance > minDistance && distance < maxDistance)
            {
                smallestDistance = distance;
                target = availableTarget;
            }
        }

        return target;
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if(_currentTarget != null)
        {
            if (_currentTarget.LaserAttackPoint == null)
            {
                OnTargetBecameNull?.Invoke();
            }
            else
            {
                TurnToTarget();
            }        
        }      
    }

    private void TurnToTarget()
    {
        if (_currentTarget.LaserAttackPoint != null)
        {
            if (_currentTarget.Transform.position.x > transform.position.x && !_isFacingRight)
                Flip();
            else if (_currentTarget.Transform.position.x < transform.position.x && _isFacingRight)
                Flip();
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
