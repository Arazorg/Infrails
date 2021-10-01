using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public static WeaponSpawner Instance;

    [SerializeField] private GameObject _weaponPrefab;

    public Weapon SpawnWeapon(WeaponData weaponData)
    {
        GameObject weaponObject = Instantiate(_weaponPrefab);
        Weapon weapon = SetWeaponComponentByType(weaponObject, weaponData.Type);
        weapon.Init(weaponData);
        return weapon;
    }

    private Weapon SetWeaponComponentByType(GameObject weapon, WeaponData.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponData.WeaponType.Rifle:
                return weapon.AddComponent<Rifle>();
            case WeaponData.WeaponType.Shotgun:
                return weapon.AddComponent<Shotgun>();
            case WeaponData.WeaponType.BurstRifle:
                return weapon.AddComponent<BurstRifle>();
            default:
                return null;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
