using UnityEngine;

public class UnitData : ScriptableObject
{
    [SerializeField] private RuntimeAnimatorController _animatorController;
    [SerializeField] private Color _unitColor;
    [SerializeField] private Vector2 _colliderOffset;
    [SerializeField] private Vector2 _colliderSize;
    [SerializeField] private string _unitName;
    [SerializeField] private int _maxHealth;

    public RuntimeAnimatorController AnimatorController => _animatorController;

    public Color UnitColor => _unitColor;

    public Vector2 ColliderOffset => _colliderOffset;

    public Vector2 ColliderSize => _colliderSize;

    public string UnitName => _unitName;

    public int MaxHealth => _maxHealth;
}
