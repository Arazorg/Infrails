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
    private EnemyData _staticEnemyData;
    private EnemyData _eggData;
    private EnemyData _mainManeCrystalData;
    private List<Enemy> _currentDestroyableObjects = new List<Enemy>();
    private GameObject _player;

    public delegate void TargetInit(GameObject target);

    public event TargetInit OnTargetInit;

    public GameObject Player
    {
        get
        {
            return _player;
        }
        set
        {
            _player = value;
            OnTargetInit?.Invoke(_player);
        }
    }

    public void SetEnemiesData(BiomeData biomeData)
    {
        _flyingEnemiesData = biomeData.FlyingEnemiesData;
        _staticEnemyData = biomeData.StaticEnemyData;
        _mainManeCrystalData = biomeData.MainManeCrystalData;
        _eggData = biomeData.EggData;
        _currentDestroyableObjects.Clear();
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
                    _currentDestroyableObjects.Add(SpawnEnemy(_eggData, spawnPoint));
                }
                else
                {
                    int dataNumber = Random.Range(0, _destroyableObjectsData.Count);
                    while (dataNumber == previousDataNumber)
                        dataNumber = Random.Range(0, _destroyableObjectsData.Count);

                    _currentDestroyableObjects.Add(SpawnEnemy(_destroyableObjectsData[dataNumber], spawnPoint));
                    previousDataNumber = dataNumber;
                }
            }
        }
    }

    public void SpawnStaticEnemy(List<Transform> teleportationPoints)
    {
        StaticEnemy staticEnemy = SpawnEnemy(_staticEnemyData, teleportationPoints[0]) as StaticEnemy;
        staticEnemy.InitScripts(teleportationPoints, _currentDestroyableObjects);
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
            _currentDestroyableObjects.Add(SpawnEnemy(_mainManeCrystalData, spawnPoint));
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
        enemy.Init(data, spawnPoint, _player);
        return enemy;
    }
}
