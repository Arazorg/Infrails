using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Standart Level", fileName = "New Level")]
public class BiomeData : ScriptableObject
{
    [SerializeField] private string _biomeName;
    [SerializeField] private Color _biomeColor;
    [SerializeField] private List<GameObject> _railsPrefabs;

    [Header("Sprites")]
    [SerializeField] private Sprite _biomeUISprite;
    [SerializeField] private Sprite _floorSprite;
    [SerializeField] private Sprite _endWallSprite;
    [SerializeField] private Sprite _plateSprite;
    [SerializeField] private Sprite _endWallFloorSprite;

    [Header("Enemies Data")]
    [SerializeField] private List<EnemyData> _flyingEnemiesData;
    [SerializeField] private List<EnemyData> _staticEnemiesData;
    [SerializeField] private EnemyData _eggData;
    [SerializeField] private EnemyData _maneCrystalData;

    public string BiomeName => _biomeName;

    public Sprite BiomeUISprite => _biomeUISprite;

    public Sprite FloorSprite => _floorSprite;

    public Sprite EndWallSprite => _endWallSprite;

    public Sprite PlateSprite => _plateSprite;

    public Sprite EndWallFloorSprite => _endWallFloorSprite;

    public Color BiomeColor => _biomeColor;

    public List<GameObject> RailsPrefabs => _railsPrefabs;

    public List<EnemyData> FlyingEnemiesData => _flyingEnemiesData;

    public List<EnemyData> StaticEnemiesData => _staticEnemiesData;

    public EnemyData EggData => _eggData;

    public EnemyData ManeCrystalData => _maneCrystalData;
}
