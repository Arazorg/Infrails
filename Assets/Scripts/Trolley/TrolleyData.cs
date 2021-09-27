using UnityEngine;

[CreateAssetMenu(menuName = "Trolleys/Standart Trolley", fileName = "New Trolley")]
public class TrolleyData : ItemData
{
    [SerializeField] private RuntimeAnimatorController _animatorController;
    [SerializeField] private Vector2 _colliderSize;
    [SerializeField] private Vector2 _colliderOffset;
    [SerializeField] private int _speed;

    public RuntimeAnimatorController AnimatorController => _animatorController;

    public Vector2 ColliderSize => _colliderSize;

    public Vector2 ColliderOffset => _colliderOffset;

    public int Speed => _speed;
}
