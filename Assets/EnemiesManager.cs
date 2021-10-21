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
    private List<EnemyData> _staticEnemiesData = new List<EnemyData>();
    private EnemyData _eggData;
    private EnemyData _mainManeCrystalData;
    private GameObject _target;

    public delegate void TargetInit(GameObject target);

    public event TargetInit OnTargetInit;

    public GameObject Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
            OnTargetInit?.Invoke(_target);
        }
    }

    public void SetEnemiesData(BiomeData biomeData)
    {
        _flyingEnemiesData = biomeData.FlyingEnemiesData;
        _staticEnemiesData = biomeData.StaticEnemiesData;
        _mainManeCrystalData = biomeData.MainManeCrystalData;
        _eggData = biomeData.EggData;
    }

    public void SpawnDestroyableObjects(List<Transform> spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            float maneCrystalSpawnChance = 0.33f;

            if (Random.Range(0f, 1f) < maneCrystalSpawnChance)
            {
                SpawnManeCrystal(spawnPoint);
            }
            else
            {
                float eggSpawnChance = 0.33f;

                if (Random.Range(0f, 1f) < eggSpawnChance)
                {
                    SpawnEnemy(_eggData, spawnPoint);
                }
                else
                {
                    int dataNumber = Random.Range(0, _destroyableObjectsData.Count);
                    SpawnEnemy(_destroyableObjectsData[dataNumber], spawnPoint);
                }
            }
        }
    }

    public void SpawnStaticEnemies(List<Transform> spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            int dataNumber = Random.Range(0, _staticEnemiesData.Count);
            SpawnEnemy(_staticEnemiesData[dataNumber], spawnPoint);
        }
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
        float spawnChanceMainManeCrystal = 0.35f;

        if (Random.Range(0f, 1f) < spawnChanceMainManeCrystal)
        {
            SpawnEnemy(_mainManeCrystalData, spawnPoint);
        }
        else
        {
            int dataNumber = Random.Range(0, _maneCrystalsData.Count);
            SpawnEnemy(_maneCrystalsData[dataNumber], spawnPoint);
        }
    }

    private void SpawnEnemy(EnemyData data, Transform spawnPoint)
    {
        Enemy enemy = _enemyFactory.GetEnemy(data.Prefab, spawnPoint);
        enemy.Init(data, spawnPoint, _target);
    }
}
