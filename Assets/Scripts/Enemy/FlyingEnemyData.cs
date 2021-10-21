using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Flying Enemy", fileName = "New Flying Enemy")]
public class FlyingEnemyData : AttackingEnemyData
{
    [SerializeField] private Vector2 _bulletSpawnPosition;
    [SerializeField] private Bullet _bulletPrefab;

    public Vector2 BulletSpawnPosition => _bulletSpawnPosition;

    public Bullet BulletPrefab => _bulletPrefab;
}
