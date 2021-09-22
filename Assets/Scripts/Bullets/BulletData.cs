using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Standart Bullet", fileName = "New Bullet")]
public class BulletData : ScriptableObject
{
    [SerializeField] private List<BulletElement> bulletsSprites;
    [SerializeField] private string bulletTag;
    [SerializeField] private Vector2 colliderSize;
    [SerializeField] private Vector2 colliderOffset;

    public List<BulletElement> BulletsSprites
    {
        get { return bulletsSprites; }
    }

    public string BulletTag
    {
        get { return bulletTag; }
    }

    public Vector2 ColliderSize
    {
        get { return colliderSize; }
    }

    public Vector2 ColliderOffset
    {
        get { return colliderOffset; }
    }

    [Serializable]
    public struct BulletElement
    {
        public ElementsResistance.Elements element;
        public Sprite bulletSprite;
        public Color bulletColor;
    }
}
