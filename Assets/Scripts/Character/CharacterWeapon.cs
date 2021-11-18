using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    private Weapon _currentWeapon;
    private Element _currentElement;

    public delegate void ElementChanged(Element element);

    public event ElementChanged OnElementChanged;

    public Weapon CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }

    public Element CurrentElement => _currentElement;

    public void Init(CharacterData characterData)
    {
        _currentElement = LevelSpawner.Instance.CurrentBiomeData.BiomeElement;
        SpawnStartWeapon(characterData);
    }
    
    public void SetWeaponElement(Element element)
    {
        _currentElement = element;
        _currentWeapon.SetElement(element.ElementType);
        OnElementChanged?.Invoke(element);
    }

    public void ChangeWeapon(WeaponData data)
    {
        _currentWeapon.Init(data);
    }

    private void SpawnStartWeapon(CharacterData data)
    {
        _currentWeapon = GetComponent<WeaponFactory>().GetWeapon(data.CharacterStartWeapon.Prefab, transform);
        _currentWeapon.Init(data.CharacterStartWeapon);
        _currentWeapon.SetParentAndOffset(transform, data.WeaponSpawnPoint);
        _currentWeapon.SetHands(data.Hands);
        _currentWeapon.CurrentElement = _currentElement.ElementType;
        OnElementChanged?.Invoke(_currentElement);
    }
}
