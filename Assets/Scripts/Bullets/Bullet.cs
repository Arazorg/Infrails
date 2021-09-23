using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;

    private BulletData _data;
    private BoxCollider2D _boxCollider2D;
    private int _damage;
    private float _critChance;

    public int Damage => _damage;

    public float CritChance => _critChance;

    public void Init(BulletData data, int damage, float critChance, ElementsResistance.Elements element)
    {
        float delayToDestroy = 5f;

        _data = data;
        _damage = damage;
        _critChance = critChance;
        GetComponent<SpriteRenderer>().sprite = GetSpriteByElement(element);
        GetComponent<BoxCollider2D>().size = _data.ColliderSize;
        GetComponent<BoxCollider2D>().offset = _data.ColliderOffset;
        SetParticle(GetColorByElement(element));
        Destroy(gameObject, delayToDestroy);
    }

    public void DestroyBullet()
    {
        _boxCollider2D.enabled = false;
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        Destroy(gameObject);
    }

    private void SetParticle(Color color)
    {
        var particleSettings = _explosionPrefab.GetComponent<ParticleSystem>().main;
        particleSettings.startColor = color;
        gameObject.tag = _data.BulletTag;
    }


    private Sprite GetSpriteByElement(ElementsResistance.Elements element)
    {
        return _data.BulletsSpritesByElements.Where(s => s.Element == element).FirstOrDefault().BulletSprite;
    }

    private Color GetColorByElement(ElementsResistance.Elements element)
    {
        return _data.BulletsSpritesByElements.Where(s => s.Element == element).FirstOrDefault().BulletColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string obstacleTag = "Obstacle";
        string bulletTag = "Bullet";

        if (!collision.tag.Contains(bulletTag))
        {
            if (collision.CompareTag(obstacleTag))
            {
                DestroyBullet();
            }              
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
