using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    private CharacterData _characterData;
    private Weapon _currentWeapon;
    private Element _currentElement;

    public delegate void ElementChanged(Element element);

    public event ElementChanged OnElementChanged;

    public delegate void WeaponChanged(WeaponData weaponData);

    public event WeaponChanged OnWeaponChanged;

    public Weapon CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }

    public Element CurrentElement => _currentElement;

    public void Init(CharacterData characterData)
    {
        _characterData = characterData;
        _currentElement = LevelSpawner.Instance.CurrentBiomeData.BiomeElement;
        SpawnWeapon(characterData.CharacterStartWeapon);
    }

    public void SetWeaponElement(Element element)
    {
        _currentElement = element;
        _currentWeapon.SetElement(element.ElementType);
        OnElementChanged?.Invoke(element);
    }

    public void SpawnWeapon(WeaponData data)
    {
        if (_currentWeapon != null)
            Destroy(_currentWeapon.gameObject);

        _currentWeapon = GetComponent<WeaponFactory>().GetWeapon(data.Prefab, transform);
        _currentWeapon.Init(data);
        _currentWeapon.SetParentAndOffset(transform, _characterData.WeaponSpawnPoint);
        _currentWeapon.SetHands(_characterData.Hands);
        _currentWeapon.CurrentElement = _currentElement.ElementType;
        OnElementChanged?.Invoke(_currentElement);
        OnWeaponChanged?.Invoke(data);
    }

}
