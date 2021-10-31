using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance;

    [Header("Enemies Data")]
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private List<EnemyData> _maneCrystalsData;
    [SerializeField] private List<EnemyData> _destroyableObjectsData;

    private List<EnemyData> _flyingEnemiesData = new List<EnemyData>();
    private List<IEnemyLaserTarget> _staticEnemyTargets = new List<IEnemyLaserTarget>();
    private EnemyData _staticEnemyData;
    private EnemyData _eggData;
    private EnemyData _mainManeCrystalData;
    private GameObject _character;

    private StaticEnemy _currentStaticEnemy;

    public delegate void CharacterAvailable(GameObject character);

    public event CharacterAvailable OnCharacterAvailable;

    public GameObject Character
    {
        get
        {
            return _character;
        }
        set
        {
            _character = value;
            OnCharacterAvailable?.Invoke(_character);
        }
    }

    public void SetEnemiesData(BiomeData biomeData)
    {
        _flyingEnemiesData = biomeData.FlyingEnemiesData;
        _staticEnemyData = biomeData.StaticEnemyData;
        _mainManeCrystalData = biomeData.MainManeCrystalData;
        _eggData = biomeData.EggData;
        _staticEnemyTargets.Clear();
    }

    public void SpawnDestroyableObjects(List<Transform> spawnPoints)
    {
        float previousDataNumber = -1;

        foreach (var spawnPoint in spawnPoints)
        {
            float maneCrystalSpawnChance = 0.3f;

            if (Random.Range(0f, 1f) < maneCrystalSpawnChance)
            {
                SpawnManeCrystal(spawnPoint);
            }
            else
            {
                float eggSpawnChance = 0.3f;

                if (Random.Range(0f, 1f) < eggSpawnChance)
                {
                    _staticEnemyTargets.Add(SpawnEnemy(_eggData, spawnPoint) as Egg);
                }
                else
                {
                    int dataNumber = Random.Range(0, _destroyableObjectsData.Count);
                    while (dataNumber == previousDataNumber)
                        dataNumber = Random.Range(0, _destroyableObjectsData.Count);

                    _staticEnemyTargets.Add(SpawnEnemy(_destroyableObjectsData[dataNumber], spawnPoint) as DestroyableKit);
                    previousDataNumber = dataNumber;
                }
            }
        }
    }

    public void SpawnStaticEnemy(List<Transform> teleportationPoints)
    {
        if (_currentStaticEnemy != null)
            Destroy(_currentStaticEnemy.gameObject);

        _currentStaticEnemy = SpawnEnemy(_staticEnemyData, teleportationPoints[1]) as StaticEnemy;
        _currentStaticEnemy.InitScripts(teleportationPoints, _staticEnemyTargets);
    }

    public void SpawnFlyingEnemies(List<Transform> spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            int dataNumber = Random.Range(0, _flyingEnemiesData.Count);
            SpawnEnemy(_flyingEnemiesData[dataNumber], spawnPoint);
        }
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
        float spawnChanceMainManeCrystal = 0.25f;

        if (Random.Range(0f, 1f) < spawnChanceMainManeCrystal)
        {
            _staticEnemyTargets.Add(SpawnEnemy(_mainManeCrystalData, spawnPoint) as ManeCrystal);
        }
        else
        {
            int dataNumber = Random.Range(0, _maneCrystalsData.Count);
            SpawnEnemy(_maneCrystalsData[dataNumber], spawnPoint);
        }
    }

    private Enemy SpawnEnemy(EnemyData data, Transform spawnPoint)
    {
        Enemy enemy = _enemyFactory.GetEnemy(data.Prefab, spawnPoint);
        enemy.Init(data, spawnPoint, _character);
        return enemy;
    }
}
