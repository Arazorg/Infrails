using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance;

    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private List<BiomeData> _biomesData;
    [SerializeField] private LevelInfoPanelUI _pauseLevelInfoPanel;
    [SerializeField] private LevelInfoPanelUI _levelInfoPanel;

    private List<BiomeData> _currentLevelBiomesData;
    private BiomeData _currentBiomeData;
    private Biome _currentBiome;
    private Rail _currentBiomeFinishRail;
    private Rail _currentBiomeStartRail;
    private Vector3 _nextBiomeSpawnPosition = Vector3.zero;
    private int _levelCounter = 1;
    private int _biomesCounter = 0;

    public delegate void LevelSpawned(int levelNumber, List<BiomeData> currentLevelBiomes);

    public event LevelSpawned OnLevelSpawned;

    public delegate void BiomeSpawned();

    public event BiomeSpawned OnBiomeSpawned;

    public BiomeData CurrentBiomeData => _currentBiomeData;

    public Rail CurrentBiomeStartRail { get => _currentBiomeStartRail; set => _currentBiomeStartRail = value; }

    public Rail CurrentBiomeFinishRail
    {
        get
        {
            return _currentBiomeFinishRail;
        }
        set
        {
            _currentBiomeFinishRail = value;
            _currentBiomeFinishRail.OnReachedEndOfLevel += SpawnBiome;
        }
    }

    public void StartSpawn()
    {
        GetBiomesOfCurrentLevel();
        LevelSpawn();
    }

    public void LevelSpawn()
    {
        SpawnBiome();
        OnLevelSpawned?.Invoke(_levelCounter, _currentLevelBiomesData.Distinct().ToList());
    }

    public void SpawnBiome()
    {
        if (_biomesCounter == _currentLevelBiomesData.Count)
        {
            _levelCounter++;
            _biomesCounter = 0;
            LevelSpawn();
        }
        else
        {
            if (_currentBiome != null)
                _currentBiome.DestroyLevel();

            _currentBiomeData = _currentLevelBiomesData[_biomesCounter];
            EnemiesManager.Instance.SetEnemiesData(_currentBiomeData);
            var level = Instantiate(_levelPrefab, _nextBiomeSpawnPosition, Quaternion.identity);
            _currentBiome = level.GetComponent<Biome>();
            var nextBiomeData = _currentLevelBiomesData[(_biomesCounter + 1) % _currentLevelBiomesData.Count];
            _nextBiomeSpawnPosition = _currentBiome.Init(_currentBiomeData, nextBiomeData);
            OnBiomeSpawned?.Invoke();
            _biomesCounter++;
        }

        if (_biomesCounter == _currentLevelBiomesData.Count)
        {
            GetBiomesOfCurrentLevel();
            _currentBiome.SetNextBiomeData(_currentLevelBiomesData[0]);
        }
    }

    public void InitLevelUI()
    {
        _pauseLevelInfoPanel.SetLevelInfoPanel(_levelCounter, _currentLevelBiomesData.Distinct().ToList());
        _levelInfoPanel.SetLevelInfoPanel(_levelCounter, _currentLevelBiomesData.Distinct().ToList());
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void GetBiomesOfCurrentLevel()
    {
        int numberBiomesDataInLevel = 3;
        List<BiomeData> biomes = new List<BiomeData>();
        while (biomes.Count != numberBiomesDataInLevel)
        {
            var biomeData = _biomesData[Random.Range(0, _biomesData.Count)];
            while (biomes.Contains(biomeData))
                biomeData = _biomesData[Random.Range(0, _biomesData.Count)];

            biomes.Add(biomeData);
        }

        float numberBiomesInLevel = 5;
        _currentLevelBiomesData = new List<BiomeData>();
        for (int i = 0; i < numberBiomesInLevel; i++)
            _currentLevelBiomesData.Add(biomes[i % numberBiomesDataInLevel]);
    }
}
