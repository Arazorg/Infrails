using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private Joystick _joystick;
    private Weapon _currentWeapon;
    private Character _character;

    private bool _isFacingRight;
    private bool _isControl;
    private float _weaponAngle;

    public Weapon CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }

    public Joystick Joystick
    {
        set
        {
            _joystick = value;
            _isControl = true;
        }
    }

    public void SpawnStartWeapon(CharacterData data, Element.Type element)
    {
        _currentWeapon = GetComponent<WeaponFactory>().GetWeapon(data.CharacterStartWeapon.Prefab, transform);
        _currentWeapon.Init(data.CharacterStartWeapon);
        _currentWeapon.SetParentAndOffset(transform, data.WeaponSpawnPoint);
        _currentWeapon.SetHands(data.Hands);
        _currentWeapon.CurrentElement = element;
        _isFacingRight = true;
    }

    public void SpawnWeapon(WeaponData data)
    {
        _currentWeapon.Init(data);
    }

    private void Start()
    {
        _character = GetComponent<Character>();
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
                _currentWeapon.Rotate(_weaponAngle);
                _currentWeapon.IsAttack = true && (!_character.IsDeath);
            }
            else
            {
                _currentWeapon.Rotate(_weaponAngle);
                _currentWeapon.IsAttack = false;
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
}
