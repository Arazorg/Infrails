using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Static Enemy", fileName = "New Static Enemy")]
public class StaticEnemyData : AttackingEnemyData
{
    [SerializeField] private RuntimeAnimatorController _laserAnimatorController;
    [SerializeField] private RuntimeAnimatorController _teleportationAnimatorController;
    [SerializeField] private Vector2 _weaponSpawnPosition;

    public RuntimeAnimatorController LaserAnimatorController => _laserAnimatorController;

    public RuntimeAnimatorController TeleportationAnimatorController => _teleportationAnimatorController;

    public Vector2 WeaponSpawnPosition => _weaponSpawnPosition;

}
