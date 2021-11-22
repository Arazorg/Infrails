using UnityEngine;

public abstract class PlayerBullet : Bullet, IHit
{
    public abstract void Accept(Transform target);

    public abstract void Accept(Transform target, IDebuffVisitor hitableVisitor);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EndWall endWall))
            Accept(collision.transform);
    }
}
