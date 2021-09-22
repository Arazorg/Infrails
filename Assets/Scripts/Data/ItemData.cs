using UnityEngine;

[CreateAssetMenu(menuName = "Items/Standart Item", fileName = "New Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private Color _itemColor;
    [SerializeField] private Sprite _itemSpriteUI;
    [SerializeField] private bool _isStartingItem;

    public string ItemName => _itemName;

    public Sprite ItemSpriteUI => _itemSpriteUI;

    public Color ItemColor => _itemColor;

    public bool IsStartingItem => _isStartingItem;
}
