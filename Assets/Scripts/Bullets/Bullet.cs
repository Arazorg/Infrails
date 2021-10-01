using System.Linq;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected BoxCollider2D BoxCollider2D;
    protected Rigidbody2D Rigidbody;
    protected BulletData Data;

    [SerializeField] protected GameObject ExplosionPrefab;
    
    private int _damage;
    private int _bulletSpeed;
    private float _critChance;

    public int Damage => _damage;

    public int BulletSpeed { get => _bulletSpeed;  set => _bulletSpeed = value; }

    public float CritChance => _critChance;


    public void Init(BulletData data, int damage, float critChance, Element.Type elementType)
    {
        Data = data;
        _damage = damage;
        _critChance = critChance;
        Rigidbody = GetComponent<Rigidbody2D>();
        SetBoxColliderParams();
        SetSpriteByElementType(elementType);
        SetParticleColor(GetColorByElementType(elementType));
    }

    public abstract void BulletHit(Collider2D collision);

    protected void DestroyBullet()
    {
        Destroy(gameObject);
    }

    protected void SpawnExplosionParticle()
    {
        BoxCollider2D.enabled = false;
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }

    private void SetBoxColliderParams()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        BoxCollider2D.size = Data.ColliderSize;
        BoxCollider2D.offset = Data.ColliderOffset;
    }

    private void SetSpriteByElementType(Element.Type element)
    {
        var sprite = Data.BulletsSpritesByElements.Where(s => s.Element == element).FirstOrDefault().BulletSprite;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void SetParticleColor(Color color)
    {
        var particleSettings = ExplosionPrefab.GetComponent<ParticleSystem>().main;
        particleSettings.startColor = color;
        gameObject.tag = Data.BulletTag;
    }

    private Color GetColorByElementType(Element.Type element)
    {
        return Data.BulletsSpritesByElements.Where(s => s.Element == element).FirstOrDefault().BulletColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string obstacleTag = "Obstacle";
        string bulletTag = "Bullet";

        if (!collision.transform.tag.Contains(bulletTag))
        {
            if (collision.transform.CompareTag(obstacleTag))
                BulletHit(collision);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
