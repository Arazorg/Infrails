using UnityEngine;

public class RicochetBullet : Bullet
{
    public override void BulletHit(Transform target)
    {
        HideBullet();
    }
}
