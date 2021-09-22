using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public static LevelSpawner Instance;

    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private List<BiomeData> _biomesData;
    [SerializeField] private LevelInfoUI _levelInfoUI;

    private List<BiomeData> _currentLevelBiomes;
    private BiomeData _currentBiomeData;
    private BiomeData _nextBiomeData;
    private Biome _currentBiome;
    private Rail _currentBiomeFinishRail;
    private Rail _currentBiomeStartRail;
    private Vector3 _nextBiomeSpawnPosition = Vector3.zero;
    private int _levelCounter = 1;
    private int _biomesCounter = 0;

    public Rail CurrentBiomeStartRail { get => _currentBiomeStartRail; set => _currentBiomeStartRail = value; }

    public Rail CurrentBiomeFinishRail
    {
        get { return _currentBiomeFinishRail; }
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
        _levelInfoUI.Show(_levelCounter, _currentLevelBiomes);
    }

    public void SpawnBiome()
    {
        float countBiomesInLevel = 5;

        if(_biomesCounter == countBiomesInLevel)
        {
            _levelCounter++;
            _biomesCounter = 0;
            LevelSpawn();
        }
        else
        {
            if (_currentBiome != null)
            {
                _currentBiome.DestroyLevel();
            }

            GetBiomeData();
            var level = Instantiate(_levelPrefab, _nextBiomeSpawnPosition, Quaternion.identity);
            _currentBiome = level.GetComponent<Biome>();
            _nextBiomeSpawnPosition = _currentBiome.Init(_currentBiomeData, _nextBiomeData);
            _biomesCounter++;
        }

        if(_biomesCounter == countBiomesInLevel)
        {
            GetBiomesOfCurrentLevel();
            GetBiomeData();
            _currentBiome.SetNextBiomeData(_nextBiomeData);
        }
    }

    public void ShowLevelUI()
    {
        _levelInfoUI.Show(_levelCounter, _currentLevelBiomes);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }          
        else
        {
            Instance = this;
        }            
    }

    private void GetBiomesOfCurrentLevel()
    {
        _currentLevelBiomes = new List<BiomeData>();

        float countBiomesDataInLevel = 3;
        while (_currentLevelBiomes.Count != countBiomesDataInLevel)
        {
            var biomeData = _biomesData[Random.Range(0, _biomesData.Count)];
            while (_currentLevelBiomes.Contains(biomeData))
            {
                biomeData = _biomesData[Random.Range(0, _biomesData.Count)];
            }
            _currentLevelBiomes.Add(biomeData);
        }
    }

    private void GetBiomeData()
    {
        if(_nextBiomeData != null)
        {
            _currentBiomeData = _nextBiomeData;
        }
        else
        {
            _currentBiomeData = _currentLevelBiomes[Random.Range(0, _currentLevelBiomes.Count)];
        }

        _nextBiomeData = _currentLevelBiomes[Random.Range(0, _currentLevelBiomes.Count)];
        while (_currentBiomeData == _nextBiomeData)
        {
            _nextBiomeData = _currentLevelBiomes[Random.Range(0, _currentLevelBiomes.Count)];
        }
    }
}
