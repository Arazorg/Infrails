using UnityEngine;

public class RicochetBullet : Bullet
{
    public override void BulletHit(Collider2D collision)
    {
        SpawnExplosionParticle();
        DestroyBullet();
    }
}
