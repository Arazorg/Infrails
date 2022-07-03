using System;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private Character _target;
    private Vector2 _targetCenterOffset;
    private float _speed = 55f;
    private bool _isHoming;

    public Character Target
    {
        set
        {
            _target = value;
            _isHoming = true;
            _targetCenterOffset = new Vector2(0, 2.5f);
        }
    }

    private void FixedUpdate()
    {
        if (_isHoming)
        {
            var centerPosition = (Vector2) _target.CenterPoint.position + _targetCenterOffset;
            Vector2 direction = centerPosition - Rigidbody.position;
            direction.Normalize();
            Rigidbody.velocity = direction * _speed;
        }
    }
}