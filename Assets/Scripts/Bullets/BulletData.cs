using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Standart Bullet", fileName = "New Bullet")]
public class BulletData : ScriptableObject
{
    [SerializeField] private Bullet _prefab;
    [SerializeField] private BulletType _type;
    [SerializeField] private Sprite _mainSprite;
    [SerializeField] private List<BulletElement> _bulletsSpritesByElements;
    [SerializeField] private Vector2 _colliderSize;
    [SerializeField] private Vector2 _colliderOffset;
    [SerializeField] private string _bulletTag;

    public enum BulletType
    {
        EnemyBullet,
        SimpleBullet,
        HomingBullet,
        Arrow,
        Missile,
        Cannonball
    }

    public Bullet Prefab => _prefab;

    public BulletType Type => _type;

    public List<BulletElement> BulletsSpritesByElements => _bulletsSpritesByElements;

    public Sprite MainSprite => _mainSprite;

    public Vector2 ColliderSize => _colliderSize;

    public Vector2 ColliderOffset => _colliderOffset;

    public string BulletTag => _bulletTag;

    [Serializable]
    public struct BulletElement
    {
        public Element.Type Element;
        public Sprite BulletSprite;
        public Color BulletColor;
    }
}
