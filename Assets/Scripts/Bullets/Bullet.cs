using System;
using System.Linq;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected GameObject ExplosionPrefab;

    protected Rigidbody2D Rigidbody;
    protected BulletData Data;

    private int _damage;
    private float _critChance;

    public int Damage => _damage;

    public float CritChance => _critChance;

    public void Init(BulletData data, WeaponData weaponData, Element.Type elementType)
    {
        Data = data;
        SetBulletPhysic();
        SetSpriteByElement(elementType);
        SetParticleColor(GetColorByElementType(elementType));
        SetCharacteristics(weaponData);
        transform.localScale = new Vector2(weaponData.BulletScaleFactor, weaponData.BulletScaleFactor);
    }

    public void Init(BulletData data, AttackingEnemyData enemyData)
    {

        Data = data;
        SetBulletPhysic();
        SetSpriteByElement(Element.Type.None);
        SetParticleColor(Color.red);
        _damage = enemyData.Damage;
    }

    public abstract void BulletHit(Collider2D collision);

    protected void DestroyBullet()
    {
        gameObject.SetActive(false);
    }

    protected void SpawnExplosionParticle()
    {
        GameObject explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
    }

    private void SetBulletPhysic()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        var boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = Data.ColliderSize;
        boxCollider2D.offset = Data.ColliderOffset;
    }

    private void SetSpriteByElement(Element.Type element)
    {
        if (Data.BulletsSpritesByElements.Count != 0)
        {
            var sprite = Data.BulletsSpritesByElements.Where(s => s.Element == element).FirstOrDefault().BulletSprite;
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Data.MainSprite;
        }
    }

    private void SetParticleColor(Color color)
    {
        var particleSettings = ExplosionPrefab.GetComponent<ParticleSystem>().main;
        particleSettings.startColor = color;
        gameObject.tag = Data.BulletTag;
    }

    private void SetCharacteristics(WeaponData weaponData)
    {
        _damage = weaponData.Damage;
        _critChance = weaponData.CritChance;
    }

    private Color GetColorByElementType(Element.Type element)
    {
        return Data.BulletsSpritesByElements.Where(s => s.Element == element).FirstOrDefault().BulletColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string obstacleTag = "Obstacle";

        if (collision.transform.CompareTag(obstacleTag))
            BulletHit(collision);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
