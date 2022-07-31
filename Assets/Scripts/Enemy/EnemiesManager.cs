using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance;

    [Header("Enemies Data")] [SerializeField]
    private EnemyFactory _enemyFactory;

    [SerializeField] private List<EnemyData> _maneCrystalsData;
    [SerializeField] private EnemyData _firstAidKitData;
    [SerializeField] private EnemyData _repairKitData;

    private List<EnemyData> _flyingEnemiesData = new List<EnemyData>();
    private List<IEnemyLaserTarget> _staticEnemyTargets = new List<IEnemyLaserTarget>();
    private List<FlyingEnemy> _currentFlyingEnemies = new List<FlyingEnemy>();
    private EnemyData _staticEnemyData;
    private EnemyData _eggData;
    private EnemyData _mainManeCrystalData;
    private EnemyData _minimalDamageManeCrystalData;
    private Character _character;
    private StaticEnemy _currentStaticEnemy;
    private LevelData _levelData;

    public delegate void CharacterAvailable(Character character);

    public event CharacterAvailable OnCharacterAvailable;

    public Character Character
    {
        get { return _character; }
        set
        {
            _character = value;
            OnCharacterAvailable?.Invoke(_character);
        }
    }

    public void SetLevelData(LevelData levelData)
    {
        _levelData = levelData;
    }

    public void SetEnemiesData(BiomeData biomeData)
    {
        _currentFlyingEnemies.Clear();
        _flyingEnemiesData = biomeData.FlyingEnemiesData;
        _staticEnemyData = biomeData.StaticEnemyData;
        _mainManeCrystalData = biomeData.MainManeCrystalData;
        _minimalDamageManeCrystalData = biomeData.MinimalDamageManeCrystalData;
        _eggData = biomeData.EggData;
        _staticEnemyTargets.Clear();
    }

    public void SpawnDestroyableObjects(List<Transform> spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            float maneCrystalSpawnChance = 0.4f;

            if (Random.Range(0f, 1f) < maneCrystalSpawnChance)
            {
                SpawnManeCrystal(spawnPoint);
            }
            else
            {
                float eggSpawnChance = 0.6f;

                if (Random.Range(0f, 1f) < eggSpawnChance)
                {
                    _staticEnemyTargets.Add(SpawnEnemyToParent(_eggData, spawnPoint) as Egg);
                }
                else
                {
                    float repairKitSpawnChance = 0.3f;
                    if (Random.Range(0f, 1f) < repairKitSpawnChance)
                        _staticEnemyTargets.Add(SpawnEnemyToParent(_repairKitData, spawnPoint) as DestroyableKit);
                    else
                        _staticEnemyTargets.Add(SpawnEnemyToParent(_firstAidKitData, spawnPoint) as DestroyableKit);
                }
            }
        }
    }

    public void SpawnStaticEnemy(List<Transform> teleportationPoints)
    {
        if (_currentStaticEnemy != null)
            Destroy(_currentStaticEnemy.gameObject);

        _currentStaticEnemy = SpawnEnemyToParent(_staticEnemyData, teleportationPoints[0]) as StaticEnemy;
        if (!CurrentGameInfo.Instance.IsInfinite)
            _currentStaticEnemy.SetEnemyLevel(_levelData.EnemiesLevel);

        _currentStaticEnemy.InitScripts(teleportationPoints, _staticEnemyTargets);
    }

    public void SpawnFlyingEnemies(List<Transform> spawnPoints)
    {
        int maxNumberOfEnemies = 10;
        int levelsForBonusEnemy = 3;
        int biomesForBonusEnemyInfiniteMode = 3;

        int bonusNumbersOfEnemies = PlayerProgress.Instance.LevelNumber / levelsForBonusEnemy;
        if (CurrentGameInfo.Instance.IsInfinite)
            bonusNumbersOfEnemies = CurrentGameInfo.Instance.ReachedBiomeNumber / biomesForBonusEnemyInfiniteMode;

        if (bonusNumbersOfEnemies > maxNumberOfEnemies)
            bonusNumbersOfEnemies = maxNumberOfEnemies;

        foreach (var spawnPoint in spawnPoints)
        {
            if (_currentFlyingEnemies.Count <= maxNumberOfEnemies + bonusNumbersOfEnemies && spawnPoint != null)
            {
                int dataNumber = Random.Range(0, _flyingEnemiesData.Count);
                FlyingEnemy enemy = SpawnEnemyByPosition(_flyingEnemiesData[dataNumber], spawnPoint) as FlyingEnemy;
                if (!CurrentGameInfo.Instance.IsInfinite)
                    enemy.SetEnemyLevel(_levelData.EnemiesLevel);
                
                enemy.OnFlyingEnemyDeath += RemoveFlyingEnemyFromList;
                _currentFlyingEnemies.Add(enemy);
            }
        }
    }

    public void SpawnEnemyFromEgg(Transform spawnPoint)
    {
        int dataNumber = Random.Range(0, _flyingEnemiesData.Count);
        SpawnEnemyByPosition(_flyingEnemiesData[dataNumber], spawnPoint);
    }

    public void SpawnBoss(BossData bossData, Transform spawnPoint)
    {
        
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void SpawnManeCrystal(Transform spawnPoint)
    {
        float maxBonusChance = 0.5f;

        float bonusChance = 0;

        if (CurrentGameInfo.Instance.IsInfinite)
            bonusChance = (1f / CurrentGameInfo.Instance.ReachedBiomeNumber);
        else
            bonusChance = (1f / _levelData.EnemiesLevel);

        if (bonusChance > maxBonusChance)
            bonusChance = maxBonusChance;

        float spawnChanceMainManeCrystal = 0.125f + bonusChance;

        if (Random.Range(0f, 1f) < spawnChanceMainManeCrystal)
        {
            var mainManeCrystal = SpawnEnemyToParent(_mainManeCrystalData, spawnPoint) as ManeCrystal;
            _staticEnemyTargets.Add(mainManeCrystal);
            mainManeCrystal.SetDamageX2Text();
        }
        else
        {
            int dataNumber = Random.Range(0, _maneCrystalsData.Count);
            while (_maneCrystalsData[dataNumber].EnemyElement == _mainManeCrystalData.EnemyElement ||
                   (!CurrentGameInfo.Instance.IsInfinite &&
                    _maneCrystalsData[dataNumber] == _minimalDamageManeCrystalData))
                dataNumber = Random.Range(0, _maneCrystalsData.Count);

            SpawnEnemyToParent(_maneCrystalsData[dataNumber], spawnPoint);
        }
    }

    private Enemy SpawnEnemyToParent(EnemyData data, Transform spawnPoint)
    {
        Enemy enemy = _enemyFactory.GetEnemy(data.Prefab, spawnPoint);
        enemy.Init(data, spawnPoint, _character);
        return enemy;
    }

    private Enemy SpawnEnemyByPosition(EnemyData data, Transform spawnPoint)
    {
        Enemy enemy = _enemyFactory.GetEnemyByPosition(data.Prefab, spawnPoint.position);
        enemy.Init(data, spawnPoint, _character);
        return enemy;
    }

    private void RemoveFlyingEnemyFromList(FlyingEnemy enemy)
    {
        enemy.OnFlyingEnemyDeath -= RemoveFlyingEnemyFromList;
        _currentFlyingEnemies.Remove(enemy);
    }
}