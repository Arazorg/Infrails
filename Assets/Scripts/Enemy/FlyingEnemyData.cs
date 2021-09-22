using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Flying Enemy", fileName = "New Flying Enemy")]
public class FlyingEnemyData : AttackingEnemyData
{
    [SerializeField] private Vector2 _bulletSpawnPosition;

    public Vector2 BulletSpawnPosition
    {
        get { return _bulletSpawnPosition; }
    }
}
