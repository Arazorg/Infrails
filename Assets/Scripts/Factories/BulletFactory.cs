using UnityEngine;

public class BulletFactory : GenericFactory<Bullet>
{
    public Bullet GetBullet(Bullet bulletPrefab, Transform pointToSpawn)
    {
        return GetNewInstanceByPosition(bulletPrefab, pointToSpawn.position);
    }
}
