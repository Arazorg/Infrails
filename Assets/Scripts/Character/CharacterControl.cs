using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private Joystick _joystick;
    private Character _character;
    private CharacterWeapon _characterWeapon;

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

    private void Start()
    {
        _isFacingRight = true;
        _character = GetComponent<Character>();
        _characterWeapon = GetComponent<CharacterWeapon>();
        LevelSpawner.Instance.OnLevelFinished += DisableControl;
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
                _characterWeapon.CurrentWeapon.Rotate(_weaponAngle);
                _characterWeapon.CurrentWeapon.IsAttack = true && (!_character.IsDeath);
            }
            else
            {
                _characterWeapon.CurrentWeapon.Rotate(_weaponAngle);
                _characterWeapon.CurrentWeapon.IsAttack = false;
            }
        }
    }

    private void Turn()
    {
        if (_isControl)
        {
            if (_joystick.Horizontal > 0 && !_isFacingRight)
                Flip();
            else if (_joystick.Horizontal < 0 && _isFacingRight)
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
    
    private void DisableControl()
    {
        LevelSpawner.Instance.OnLevelFinished -= DisableControl;
        _isControl = false;
        _characterWeapon.CurrentWeapon.IsAttack = false;
    }
}
