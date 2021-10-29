using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Standart Level", fileName = "New Level")]
public class BiomeData : ScriptableObject
{
    [SerializeField] private string _biomeName;
    [SerializeField] private Color _biomeColor;
    [SerializeField] private List<GameObject> _railsPrefabs;
    [SerializeField] private Element _biomeElement;

    [Header("Sprites")]
    [SerializeField] private Sprite _floorSprite;
    [SerializeField] private Sprite _endWallSprite;
    [SerializeField] private Sprite _plateSprite;
    [SerializeField] private Sprite _endWallFloorSprite;

    [Header("Enemies Data")]
    [SerializeField] private List<EnemyData> _flyingEnemiesData;
    [SerializeField] private EnemyData _staticEnemyData;
    [SerializeField] private EnemyData _eggData;
    [SerializeField] private EnemyData _mainManeCrystalData;

    public string BiomeName => _biomeName;

    public Element BiomeElement => _biomeElement;

    public Sprite FloorSprite => _floorSprite;

    public Sprite EndWallSprite => _endWallSprite;

    public Sprite PlateSprite => _plateSprite;

    public Sprite EndWallFloorSprite => _endWallFloorSprite;

    public Color BiomeColor => _biomeColor;

    public List<GameObject> RailsPrefabs => _railsPrefabs;

    public List<EnemyData> FlyingEnemiesData => _flyingEnemiesData;

    public EnemyData StaticEnemyData => _staticEnemyData;

    public EnemyData EggData => _eggData;

    public EnemyData MainManeCrystalData => _mainManeCrystalData;
}
