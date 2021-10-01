using UnityEngine;

public class Missile : Bullet
{
    [SerializeField] private GameObject _explosionPrefab;

    public override void BulletHit(Collider2D collision)
    {
        SpawnExplosion();
        DestroyBullet();
    }

    private void SpawnExplosion()
    {
        BoxCollider2D.enabled = false;
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        float explosionDamageRadius = 5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosion.transform.position, explosionDamageRadius);
        ExplosionDamage(colliders);
    }

    private void ExplosionDamage(Collider2D[] colliders)
    {
        int explosionDamage = 3;
        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.GetDamage(explosionDamage);
        }
    }
}
