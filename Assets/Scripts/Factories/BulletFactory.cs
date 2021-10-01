using UnityEngine;

public class BulletFactory : GenericFactory<Bullet>
{
    public Bullet GetBullet(Bullet bulletPrefab, Transform pointToSpawn)
    {
        return GetNewInstanceToPosition(bulletPrefab, pointToSpawn.position);
    }
}
