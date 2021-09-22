using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private BulletData _data;
    private int _damage;
    private float _critChance;

    public Vector2 ColliderOffset
    {
        get { return _data.ColliderOffset; }
    }

    public int Damage
    {
        get { return _damage; }
    }

    public float CritChance
    {
        get { return _critChance; }
    }

    public void Init(BulletData data, int damage, float critChance, ElementsResistance.Elements element)
    {
        _data = data;

        GetComponent<SpriteRenderer>().sprite = GetSpriteByElement(element);
        GetComponent<BoxCollider2D>().size = _data.ColliderSize;
        GetComponent<BoxCollider2D>().offset = ColliderOffset;

        var particleSettings = explosionPrefab.GetComponent<ParticleSystem>().main;
        particleSettings.startColor = GetColorByElement(element);
        gameObject.tag = _data.BulletTag;

        _damage = damage;
        _critChance = critChance;

        float delayToDestroy = 5f;
        Destroy(gameObject, delayToDestroy);
    }

    public void DestroyBullet()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        Destroy(gameObject);
    }

    private Sprite GetSpriteByElement(ElementsResistance.Elements element)
    {
        foreach (var bulletSprite in _data.BulletsSprites)
        {
            if (bulletSprite.element == element)
            {
                return bulletSprite.bulletSprite;
            }
        }

        return null;
    }

    private Color GetColorByElement(ElementsResistance.Elements element)
    {
        foreach (var bulletSprite in _data.BulletsSprites)
        {
            if (bulletSprite.element == element)
            {
                return bulletSprite.bulletColor;
            }
        }

        return Color.white;
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
