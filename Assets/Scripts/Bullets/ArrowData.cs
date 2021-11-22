using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullets/Standart Arrow", fileName = "New Arrow")]
public class ArrowData : BulletData
{
    [SerializeField] private List<ArrowsSprites> _stickedArrowsSprites;

    public List<ArrowsSprites> StickedArrowSprites => _stickedArrowsSprites;


    [Serializable]
    public struct ArrowsSprites
    {
        public Sprite ArrowSprite;
        public Sprite StickedArrowSprite;
    }
}
