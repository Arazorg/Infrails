using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private Joystick _joystick;
    private Weapon _weapon;
    private bool _isFacingRight;
    private bool _isControl;
    private float _weaponAngle;

    public Joystick Joystick
    {
        set 
        { 
            _joystick = value;
            _isControl = true;
        }
    }

    public void SpawnWeapon(CharacterData data, Vector2 weaponOffset)
    {
        _weapon  = WeaponSpawner.Instance.SpawnWeapon(data.CharacterStartWeapon);
        _weapon.SetParentAndOffset(transform, weaponOffset);
        _weapon.SetHands(data.Hands);
        _isFacingRight = true;
    }

    private void Update()
    {
        WeaponRotation();
        Turn();
    }

    private void WeaponRotation()
    {
        if (_isControl)
        {
            if (_joystick.Horizontal != 0f && _joystick.Vertical != 0f)
            {
                _weaponAngle = -Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg;
                _weapon.RotateAndAttack(true, _weaponAngle);
            }
            else
            {
                _weapon.RotateAndAttack(false, _weaponAngle);
            }
        }
    }

    private void Turn()
    {
        if (_isControl)
        {
            if (_joystick.Horizontal > 0 && !_isFacingRight)
            {
                Flip();
            }
            else if (_joystick.Horizontal < 0 && _isFacingRight)
            {
                Flip();
            }
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
