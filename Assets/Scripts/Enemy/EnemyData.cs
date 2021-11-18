using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Enemy", fileName = "New Enemy")]
public class EnemyData : UnitData
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private Element _enemyElement;
    [SerializeField] private AudioClip _deathAudioClip;
    [SerializeField] private Vector2 _popUpTextOffset;
    [SerializeField] private Vector2 _center;
    [SerializeField] private bool _isFlipX;

    public enum EnemyType
    {
        Flying,
        Static,
        Egg,
        Chest,
        ManeCrystal
    }

    public Enemy Prefab => _prefab;

    public Element EnemyElement => _enemyElement;

    public AudioClip DeathAudioClip => _deathAudioClip;

    public Vector2 PopUpTextOffset => _popUpTextOffset;

    public Vector2 Center => _center;

    public bool IsFlipX => _isFlipX;
}
