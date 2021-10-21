using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Enemy", fileName = "New Enemy")]
public class EnemyData : UnitData
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private Element _enemyElement;

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

    public Enemy Prefab => _prefab;

    public Element EnemyElement => _enemyElement;
}
