using UnityEngine;

public class Cannonball : Bullet
{
    public override void BulletHit(Collider2D collision)
    {
        SpawnExplosionParticle();
        DestroyBullet();
    }
}
