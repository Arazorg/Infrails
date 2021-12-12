using System.Linq;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected GameObject ExplosionPrefab;

    protected Rigidbody2D Rigidbody;
    protected BulletData Data;
    private Element.Type _elementType;
    private int _damage;
    private float _critChance;

    public Element.Type ElementType => _elementType;

    public int Damage => _damage;

    public float CritChance => _critChance;

    public Color Init(WeaponData weaponData, WeaponCharacteristics weaponCharacteristics, Element.Type elementType)
    {
        Data = weaponData.BulletData;
        SetBulletPhysic();
        _elementType = elementType;
        SetSpriteByElement(elementType);
        Color elementColor = GetColorByElementType(elementType);
        SetParticleColor(elementColor);
        SetCharacteristics(weaponCharacteristics);
        transform.localScale = new Vector2(weaponCharacteristics.BulletScaleFactor, weaponCharacteristics.BulletScaleFactor);
        return elementColor;
    }

    public void DestroyBullet()
    {
        float destroyDelay = 2f;
        Destroy(gameObject, destroyDelay);
    }

    public void HideBullet()
    {
        SpawnExplosionParticle();
        gameObject.SetActive(false);
    }

    private void SpawnExplosionParticle()
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

    private void SetCharacteristics(WeaponCharacteristics weaponCharacteristics)
    {
        _damage = weaponCharacteristics.Damage;
        _critChance = weaponCharacteristics.CritChance;
    }

    private Color GetColorByElementType(Element.Type element)
    {
        var color = Color.red;
        if (element == Element.Type.None)
            return color;

        color = Data.BulletsSpritesByElements.Where(s => s.Element == element).FirstOrDefault().BulletColor;
        return color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EndWall endWall))
            HideBullet();
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
