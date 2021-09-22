using UnityEngine;

[CreateAssetMenu(menuName = "Trolleys/Standart Trolley", fileName = "New Trolley")]
public class TrolleyData : ScriptableObject
{
    [SerializeField] private RuntimeAnimatorController animatorController;
    public RuntimeAnimatorController AnimatorController
    {
        get { return animatorController; }
    }

    [SerializeField] private Vector2 colliderSize;
    public Vector2 ColliderSize
    {
        get { return colliderSize; }
    }

    [SerializeField] private Vector2 colliderOffset;
    public Vector2 ColliderOffset
    {
        get { return colliderOffset; }
    }

    [SerializeField] private int speed;
    public int Speed
    {
        get { return speed; }
    }
}
