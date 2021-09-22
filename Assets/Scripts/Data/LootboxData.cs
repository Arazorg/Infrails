using UnityEngine;

[CreateAssetMenu(menuName = "Lootboxes/Standart Lootbox", fileName = "New Lootbox")]
public class LootboxData : ScriptableObject
{
    public enum Type
    {
        Amplification,
        Weapon,
        Skill,
        Buff,
        Money,
        ResetTime
    }

    [SerializeField] private Type _typeOfLootbox;
    [SerializeField] private RuntimeAnimatorController _animatorController;
    [SerializeField] private Sprite _mainSprite;
    [SerializeField] private Color _lootboxColor;
    [SerializeField] private string _nameKey;
    [SerializeField] private int _price;
    [SerializeField] private bool _isAdLootbox;

    public Type TypeOfLootbox => _typeOfLootbox;

    public RuntimeAnimatorController AnimatorController => _animatorController;

    public Sprite MainSprite => _mainSprite;

    public Color LootboxColor => _lootboxColor;

    public string NameKey => _nameKey;

    public int Price => _price;

    public bool IsAdLootbox => _isAdLootbox;
}
