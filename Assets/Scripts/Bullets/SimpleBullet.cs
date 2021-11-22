using UnityEngine;

public class SimpleBullet : PlayerBullet
{
    public override void Accept(Transform target)
    {
        HideBullet();
    }

    public override void Accept(Transform target, IDebuffVisitor hitableVisitor)
    {
        HideBullet();
    }
}
