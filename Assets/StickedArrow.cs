using System.Linq;
using UnityEngine;

public class StickedArrow : MonoBehaviour
{
    public void Init(ArrowData arrowData, Sprite arrowSprite, Vector2 parentScale)
    {
        var arrowsSprites = arrowData.StickedArrowSprites.Where(s => s.ArrowSprite == arrowSprite).FirstOrDefault();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = arrowsSprites.StickedArrowSprite;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        spriteRenderer.size /= parentScale;
    }
}
