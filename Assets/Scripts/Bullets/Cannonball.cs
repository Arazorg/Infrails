using UnityEngine;

public class Cannonball : Bullet
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
