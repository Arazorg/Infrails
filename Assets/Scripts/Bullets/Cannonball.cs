using UnityEngine;

public class Cannonball : PlayerBullet
{
    public override void Accept(Transform target)
    {
        HideBullet();
    }

    public override void Accept(Transform target, IDebuffVisitor hitableVisitor)
    {
        hitableVisitor.StartStunning();
        HideBullet();
    }
}
