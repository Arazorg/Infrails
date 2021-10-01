using UnityEngine;

public class WeaponFactory : GenericFactory<Weapon>
{
    public Weapon GetWeapon(Weapon weaponPrefab, Transform parent)
    {
        return GetNewInstanceToParent(weaponPrefab, parent);
    }
}
