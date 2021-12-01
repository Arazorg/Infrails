using UnityEngine;

[CreateAssetMenu(menuName = "GameShopProducts/Standart Product", fileName = "New Product")]
public class GameShopProductData : ScriptableObject
{
    [SerializeField] private string _productName;
    [SerializeField] private Sprite _productSprite;
    [SerializeField] private Color _productColor;
    [SerializeField] private Vector2 _productSpriteSize;
    [SerializeField] private Type _productType;
    [SerializeField] private int _price;

    public enum Type
    {
        WeaponLootbox,
        AmplificationLootbox,
        HealPotion,
        ElementPotion
    }

    public string ProductName => _productName;

    public Sprite ProductSprite => _productSprite;

    public Color ProductColor => _productColor;

    public Vector2 ProductSpriteSize => _productSpriteSize;

    public Type ProductType => _productType;

    public int Price => _price;

}
