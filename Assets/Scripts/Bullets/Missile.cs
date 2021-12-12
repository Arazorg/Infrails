using UnityEngine;

public class Missile : PlayerBullet
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _explosionAudioClip;

    public override void Accept(Transform target)
    {
        SpawnExplosion();
        HideBullet();
    }

    public override void Accept(Transform target, IDebuffVisitor hitableVisitor)
    {
        SpawnExplosion();
        HideBullet();
    }

    private void SpawnExplosion()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        float explosionDamageRadius = 5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosion.transform.position, explosionDamageRadius);
        AudioManager.Instance.PlayEffect(_explosionAudioClip);
        ExplosionDamage(colliders);
    }

    private void ExplosionDamage(Collider2D[] colliders)
    {
        foreach (var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.GetDamage(Damage);
        }
    }
}
