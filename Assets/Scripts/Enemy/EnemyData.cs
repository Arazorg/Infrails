using UnityEngine;

public class EnemyData : UnitData
{
    [SerializeField] private EnemyType _type;
    [SerializeField] private bool _isMove;
    [SerializeField] private bool _isStaticScale;

    public enum EnemyType
    {
        Flying,
        Static,
        Egg,
        Chest,
        Obstacle,
        Barrel,
        ManeCrystal
    }

    public EnemyType Type
    {
        get { return _type; }
    }

    public bool IsMove
    {
        get { return _isMove; }
    }

    public bool IsStaticScale
    {
        get { return _isStaticScale; }
    }
}
