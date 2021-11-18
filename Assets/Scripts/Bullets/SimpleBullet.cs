using UnityEngine;

public class SimpleBullet : Bullet
{
    public override void BulletHit(Transform target)
    {
        HideBullet();
    }
}
