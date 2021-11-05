using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Enemy", fileName = "New Enemy")]
public class EnemyData : UnitData
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private Element _enemyElement;
    [SerializeField] private bool _isSpriteFacingRight;
    [SerializeField] private AudioClip _deathAudioClip;

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

    public bool IsSpriteFacingRight => _isSpriteFacingRight;

    public AudioClip DeathAudioClip => _deathAudioClip;
}
