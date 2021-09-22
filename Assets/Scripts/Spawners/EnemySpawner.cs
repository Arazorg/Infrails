using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [SerializeField] private GameObject _flyingEnemyPrefab;
    [SerializeField] private GameObject _staticEnemyPrefab;
    [SerializeField] private GameObject _destroyableObjectPrefab;

    private List<EnemyData> _flyingEnemiesData = new List<EnemyData>();
    private List<EnemyData> _staticEnemiesData = new List<EnemyData>();
    private EnemyData _eggData;
    private EnemyData _mainManeCrystalData;
    private List<Enemy> _currentAttackingEnemies = new List<Enemy>();
    private GameObject _characterObject;
    private Character _character;

    public GameObject CharacterObject
    {
        get
        {
            return _characterObject;
        }
        set
        {
            _characterObject = value;
            _character = _characterObject.GetComponent<Character>();
            _character.OnCharacterDeath += StopAndDestroy;
        }
    }

    public void SetEnemiesData(BiomeData biomeData)
    {
        _flyingEnemiesData = biomeData.FlyingEnemiesData;
        _staticEnemiesData = biomeData.StaticEnemiesData;
        _eggData = biomeData.EggData;
        //_mainManeCrystalData = biomeData.ainManeCrystalData;
    }

    public void SpawnAttackingEnemies(List<Transform> flyingEnemiesSpawnPoints, List<Transform> staticEnemiesSpawnPoints)
    {
        foreach (var spawnPoint in flyingEnemiesSpawnPoints)
        {
            GameObject flyingEnemy = Instantiate(_flyingEnemyPrefab, spawnPoint.position, Quaternion.identity);
            int dataNumber = Random.Range(0, _flyingEnemiesData.Count);
            FlyingEnemy flyingEnemyScript = flyingEnemy.AddComponent<FlyingEnemy>();
            flyingEnemyScript.Init(_flyingEnemiesData[dataNumber], _characterObject, spawnPoint);
            _currentAttackingEnemies.Add(flyingEnemyScript);
        }

        foreach (var spawnPoint in staticEnemiesSpawnPoints)
        {
            GameObject staticEnemy = Instantiate(_staticEnemyPrefab, spawnPoint.position, Quaternion.identity);
            int dataNumber = Random.Range(0, _staticEnemiesData.Count);
            StaticEnemy staticEnemyScript = staticEnemy.AddComponent<StaticEnemy>();
            staticEnemyScript.Init(_flyingEnemiesData[dataNumber], _characterObject);
            _currentAttackingEnemies.Add(staticEnemyScript);
        }
    }

    public void SpawnEnemiesFromEgg(Transform spawnPoint)
    {
        int minCountOfEnemy = 5;
        int maxCountOfEnemy = 8;

        for (int i = 0; i < Random.Range(minCountOfEnemy, maxCountOfEnemy); i++)
        {
            GameObject flyingEnemy = Instantiate(_flyingEnemyPrefab, spawnPoint.position, Quaternion.identity);
            int dataNumber = Random.Range(0, _flyingEnemiesData.Count);
            FlyingEnemy flyingEnemyScript = flyingEnemy.AddComponent<FlyingEnemy>();
            flyingEnemyScript.Init(_flyingEnemiesData[dataNumber], _characterObject);
            _currentAttackingEnemies.Add(flyingEnemyScript);
        }
    }

    public void SpawnDestroyableObjects(List<Transform> spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {

        }
    }

    public void MoveEnemiesToSpawnPoints()
    {
        foreach (var enemy in _currentAttackingEnemies)
        {
            if (enemy is FlyingEnemy)
            {
                enemy.GetComponent<EnemyMovement>().MoveToSpawnPoint();
            }
        }

        _currentAttackingEnemies.Clear();
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

    private void StopAndDestroy()
    {
        StartCoroutine(StopingAndDestroying());
    }

    private IEnumerator StopingAndDestroying()
    {
        float delay = 0.33f;
        StopAllAttackingEnemies();
        yield return new WaitForSeconds(delay);
        KillAllAttackingEnemies();
    }

    private void StopAllAttackingEnemies()
    {
        foreach (var enemy in _currentAttackingEnemies)
        {
            if (enemy is FlyingEnemy)
            {
                enemy.GetComponent<EnemyMovement>().IsMove = false;
            }
        }

    }

    private void KillAllAttackingEnemies()
    {
        foreach (var enemy in _currentAttackingEnemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().Death();
            }
        }
    }
}
