using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Static Enemy", fileName = "New Static Enemy")]
public class StaticEnemyData : AttackingEnemyData
{
    [SerializeField] private StaticEnemyWeaponData _weaponData;
    [SerializeField] private Vector2 _weaponSpawnPosition;
    [SerializeField] private RuntimeAnimatorController _teleportationAnimatorController;

    public StaticEnemyWeaponData WeaponData => _weaponData;

    public Vector2 WeaponSpawnPosition => _weaponSpawnPosition;

    public RuntimeAnimatorController TeleportationAnimatorController => _teleportationAnimatorController;
}
