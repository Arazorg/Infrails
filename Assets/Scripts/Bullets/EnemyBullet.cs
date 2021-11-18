using UnityEngine;

public class EnemyBullet : Bullet
{
    public override void BulletHit(Transform target)
    {
        HideBullet();
    }
}
