using System.Collections;
using UnityEngine;

public class Arrow : Bullet
{
    public override void BulletHit(Collider2D collision)
    {
        DestroyBullet();
        //StickArrow(collision.transform);
    }

    private void StickArrow(Transform target)
    {
        transform.parent = target;
        
        Rigidbody.Sleep();
        StartCoroutine(DelayToDestroy());
    }

    private IEnumerator DelayToDestroy()
    {
        float delay = 1.25f;
        yield return new WaitForSeconds(delay);
        SpawnExplosionParticle();
        DestroyBullet();
    }
}
