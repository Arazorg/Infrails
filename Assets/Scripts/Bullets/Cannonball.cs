using UnityEngine;

public class Cannonball : Bullet
{
    public override void BulletHit(Transform target)
    {
        HideBullet();
    }
}
