using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticEnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyLightningLaser _weapon;

    private StaticEnemyData _staticEnemyData;
    private List<Enemy> _targets;
    private Enemy _currentTarget;
    private bool _isAttack;
    private bool _isFacingRight;

    public delegate void TargetIsNull();

    public event TargetIsNull OnTargetIsNull;

    public void Init(StaticEnemyData staticEnemyData, List<Enemy> targets)
    {
        _staticEnemyData = staticEnemyData;
        _targets = targets;
        InitWeapon();
        _isAttack = false;
        _isFacingRight = true;
    }

    public void Attack()
    {
        SetTarget();
        TurnToTarget();
        _weapon.SetTarget(_currentTarget);
    }

    private void InitWeapon()
    {
        _weapon.transform.localPosition = _staticEnemyData.WeaponSpawnPosition;
        _weapon.InitWeapon(_staticEnemyData.WeaponData);
    }

    private void Update()
    {
        if (_currentTarget == null && _isAttack)
        {
            Debug.Log("NULKLLL");
            _isAttack = false;
            _targets.Remove(_currentTarget);
            OnTargetIsNull?.Invoke();
        }          
    }

    private void SetTarget()
    {
        var targets = _targets.Where(s => s == s.IsGetDamage);
        _currentTarget = targets.FirstOrDefault();
        if(_currentTarget != null)
            _isAttack = true;
    }

    private void TurnToTarget()
    {
        if(_currentTarget != null)
        {
            if (_currentTarget.transform.position.x > transform.position.x && !_isFacingRight)
                Flip();
            else if (_currentTarget.transform.position.x < transform.position.x && _isFacingRight)
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
