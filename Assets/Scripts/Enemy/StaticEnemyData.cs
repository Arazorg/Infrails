using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Static Enemy", fileName = "New Static Enemy")]
public class StaticEnemyData : AttackingEnemyData
{
    [SerializeField] private StaticEnemyWeaponData _weaponData;
    [SerializeField] private Vector2 _weaponSpawnPosition;

    public StaticEnemyWeaponData WeaponData
    {
        get { return _weaponData; }
    }

    public Vector2 WeaponSpawnPosition
    {
        get { return _weaponSpawnPosition; }
    }
}
