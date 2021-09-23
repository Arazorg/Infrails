using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Standart Bullet", fileName = "New Bullet")]
public class BulletData : ScriptableObject
{
    [SerializeField] private List<BulletElement> _bulletsSpritesByElements;
    [SerializeField] private Vector2 _colliderSize;
    [SerializeField] private Vector2 _colliderOffset;
    [SerializeField] private string _bulletTag;

    public List<BulletElement> BulletsSpritesByElements => _bulletsSpritesByElements;

    public Vector2 ColliderSize => _colliderSize;

    public Vector2 ColliderOffset => _colliderOffset;

    public string BulletTag => _bulletTag;


    [Serializable]
    public struct BulletElement
    {
        public ElementsResistance.Elements Element;
        public Sprite BulletSprite;
        public Color BulletColor;
    }
}
