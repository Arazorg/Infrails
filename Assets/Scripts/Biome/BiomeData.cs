using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Standart Level", fileName = "New Level")]
public class BiomeData : ScriptableObject
{
    [SerializeField] private string _biomeName;
    [SerializeField] private Color _biomeColor;
    [SerializeField] private Sprite _biomeUISprite;
    [SerializeField] private Sprite _floorSprite;
    [SerializeField] private Sprite _endWallSprite;
    [SerializeField] private Sprite _plateSprite;
    [SerializeField] private Sprite _endWallFloorSprite;
    [SerializeField] private List<GameObject> _railsPrefabs;
    [SerializeField] private List<EnemyData> _flyingEnemiesData;
    [SerializeField] private List<EnemyData> _staticEnemiesData;
    [SerializeField] private EnemyData _eggData;
    [SerializeField] private EnemyData _maneCrystalData;
    [SerializeField] private float _globalLightsIntensity;

    public string BiomeName
    {
        get { return _biomeName; }
    }

    public Sprite BiomeUISprite => _biomeUISprite;
    
    public Sprite FloorSprite
    {
        get { return _floorSprite; }
    }
    
    public Sprite EndWallSprite
    {
        get { return _endWallSprite; }
    }
   
    public Sprite PlateSprite
    {
        get { return _plateSprite; }
    }
    
    public Sprite EndWallFloorSprite
    {
        get { return _endWallFloorSprite; }
    }

    public Color BiomeColor
    {
        get { return _biomeColor; }
    }

    public List<GameObject> RailsPrefabs
    {
        get { return _railsPrefabs; }
    }
  
    public List<EnemyData> FlyingEnemiesData
    {
        get { return _flyingEnemiesData; }
    }

    public List<EnemyData> StaticEnemiesData
    {
        get { return _staticEnemiesData; }
    }

    public EnemyData EggData
    {
        get { return _eggData; }
    }

    public EnemyData ManeCrystalData
    {
        get { return _maneCrystalData; }
    }

    public float GlobalLightsIntensity
    {
        get { return _globalLightsIntensity; }
    }
}
