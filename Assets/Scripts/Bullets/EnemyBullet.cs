using UnityEngine;

public class EnemyBullet : Bullet
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
