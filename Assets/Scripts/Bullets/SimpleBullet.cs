using UnityEngine;

public class SimpleBullet : Bullet
{
    public override void BulletHit(Collider2D collision)
    {
        HideBullet();
    }
}
