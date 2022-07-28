using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    private const int NumberBiomesInLevel = 3;

    public static LevelSpawner Instance;

    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private List<BiomeData> _biomesData;
    [SerializeField] private LevelInfoPanelUI _pauseLevelInfoPanel;
    [SerializeField] private LevelInfoPanelUI _levelInfoPanel;
    [SerializeField] private LevelInfoPanelUI _inifiniteModeLevelInfoPanel;
    [SerializeField] private GameStartUI _gameStartUI;
    [SerializeField] private EndOfGameUI _endOfGameUI;
    [SerializeField] private RebornUI _rebornUI;

    private List<BiomeData> _currentLevelBiomesData;
    private BiomeData _currentBiomeData;
    private Biome _currentBiome;
    private Rail _currentBiomeFinishRail;
    private Rail _currentBiomeStartRail;
    private Vector3 _nextBiomeSpawnPosition = Vector3.zero;
    private int _levelCounter = 1;
    private int _biomesCounter = 0;
    private bool _isGameInfinite;

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
        LoadLevelBiomes();
        LevelSpawn();
        _endOfGameUI.SetLenghtOfLevel(_nextBiomeSpawnPosition.y * NumberBiomesInLevel);
        InitLevelUI();
    }

    public void StartSpawnInfinite()
    {
        _isGameInfinite = true;
        SetBiomesOfCurrentLevel();
        LevelSpawn();
        InitLevelUI();
        _inifiniteModeLevelInfoPanel.Show();
    }


    public void LevelSpawn()
    {
        _rebornUI.Init(_isGameInfinite);
        SpawnBiome();
        OnLevelSpawned?.Invoke(_levelCounter, _currentLevelBiomesData.Distinct().ToList());
    }

    public void SpawnBiome()
    {
        if (_biomesCounter == _currentLevelBiomesData.Count)
        {
            if (_isGameInfinite)
            {
                _levelCounter++;
                _biomesCounter = 0;
                LevelSpawn();
                _inifiniteModeLevelInfoPanel.Show();
            }
            else
            {
                PlayerProgress.Instance.LevelNumber++;
                _endOfGameUI.SetInfo(true, _isGameInfinite);
                UIManager.Instance.UIStackPush(_endOfGameUI);
            }
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
            if(_isGameInfinite)
            {
                SetBiomesOfCurrentLevel();
                _currentBiome.SetNextBiomeData(_currentLevelBiomesData[0]);
            }
        }
    }

    private void InitLevelUI()
    {
        int level = PlayerProgress.Instance.LevelNumber;
        if(_isGameInfinite)
            level = _levelCounter;

        _pauseLevelInfoPanel.SetLevelInfoPanel(level, _currentLevelBiomesData.Distinct().ToList());
        _levelInfoPanel.SetLevelInfoPanel(level, _currentLevelBiomesData.Distinct().ToList());
        _inifiniteModeLevelInfoPanel.SetLevelInfoPanel(level, _currentLevelBiomesData.Distinct().ToList());
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void LoadLevelBiomes()
    {
        string levelsDataPath = "Specifications/Levels";
        var levelsData = Resources.LoadAll<LevelData>(levelsDataPath).ToList();
        var levelData = levelsData[PlayerProgress.Instance.LevelNumber - 1];
        _currentLevelBiomesData = levelData.Biomes;
        _gameStartUI.SetWeapons(levelData.Weapons);
        _gameStartUI.SetAmplifications(levelData.Amplifications);
        EnemiesManager.Instance.SetLevelData(levelData);
    }

    private void SetBiomesOfCurrentLevel()
    {;
        List<BiomeData> biomes = new List<BiomeData>();
        while (biomes.Count != NumberBiomesInLevel)
        {
            var biomeData = _biomesData[Random.Range(0, _biomesData.Count)];
            while (biomes.Contains(biomeData))
                biomeData = _biomesData[Random.Range(0, _biomesData.Count)];

            biomes.Add(biomeData);
        }

        _currentLevelBiomesData = new List<BiomeData>();
        for (int i = 0; i < NumberBiomesInLevel; i++)
            _currentLevelBiomesData.Add(biomes[i % NumberBiomesInLevel]);
    }
}
