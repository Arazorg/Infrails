using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Standart Static Enemy Weapon", fileName = "New Static Enemy Weapon")]

public class StaticEnemyWeaponData : ScriptableObject
{
    [SerializeField] private Sprite _weaponSprite;
    [SerializeField] private Vector2 _bulletSpawnPosition;
}
