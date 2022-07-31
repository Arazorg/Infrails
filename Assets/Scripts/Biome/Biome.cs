using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Biome : MonoBehaviour
{
    [SerializeField] private Transform _levelFloorsParentObject;

    [Header("Prefabs")] [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private GameObject _endWallPrefab;

    private BiomeData _biomeData;
    private List<Transform> _staticEnemyTeleportationPoints = new List<Transform>();
    private Rail _lastRail;
    private EndWall _endWall;
    private bool _isSimplePhase;

    public Vector3 Init(BiomeData data, BiomeData nextBiomeData, bool isSimplePhase)
    {
        _biomeData = data;
        int minLengthOfLevel = 5;
        int maxLenghtOfLevel = 7;
        Vector3 nextLevelSpawnPosition =
            CreateEnvironment(Random.Range(minLengthOfLevel, maxLenghtOfLevel), isSimplePhase);
        
        if (isSimplePhase)
        {
            SetNextBiomeData(nextBiomeData);
        }

        SetBiomeLightsState(GameConstants.TurnOn);
        return nextLevelSpawnPosition;
    }

    public void SetNextBiomeData(BiomeData nextBiomeData)
    {
        _endWall.GetComponent<EndWall>().SetEndWallEnvironment(nextBiomeData, _biomeData.EndWallFloorSprite);
    }

    public void SetBiomeLightsState(bool isState)
    {
        foreach (var floor in GetComponentsInChildren<Floor>())
        {
            if (isState)
                floor.SetNeedLightsIntensity(1f);
            else
                floor.SetNeedLightsIntensity(0f);
        }

        float endWallLightIntensity = 0.75f;
        var endWallScript = GetComponentInChildren<EndWall>();
        if (endWallScript != null)
        {
            if (isState)
                endWallScript.SetNeedLightsIntensity(endWallLightIntensity);
            else
                endWallScript.SetNeedLightsIntensity(0f);
        }
    }

    public void DestroyLevel()
    {
        float destroyDelay = 100;
        if (_isSimplePhase)
            destroyDelay = 5;

        Destroy(gameObject, destroyDelay);
    }

    private Vector3 CreateEnvironment(int lengthOfLevel, bool isSimplePhase)
    {
        _isSimplePhase = isSimplePhase;
        
        Vector3 nextFloorSpawnPosition = transform.position;
        int lastRailsPatternNumber = Random.Range(0, _biomeData.RailsPrefabs.Count);

        for (int i = 0; i < lengthOfLevel; i++)
        {
            GameObject floor = Instantiate(_floorPrefab, nextFloorSpawnPosition, Quaternion.identity,
                _levelFloorsParentObject);
            floor.GetComponent<SpriteRenderer>().sprite = _biomeData.FloorSprite;
            nextFloorSpawnPosition += floor.GetComponent<Floor>().NextFloorSpawnPoint.localPosition;
            lastRailsPatternNumber = SpawnRailsPattern(floor, i, lastRailsPatternNumber);
        }

        Vector3 nextLevelSpawnPosition = nextFloorSpawnPosition;

        if (isSimplePhase)
        {
            EnemiesManager.Instance.SpawnStaticEnemy(_staticEnemyTeleportationPoints);
            nextLevelSpawnPosition = CreateEndWall(nextFloorSpawnPosition);
        }
        else
        {
            LevelSpawner.Instance.CurrentBiomeFinishRail = _lastRail;
        }

        return nextLevelSpawnPosition;
    }

    private int SpawnRailsPattern(GameObject floor, int numberOfBiome, int lastRailsPatternNumber)
    {
        int currentRailsPatternNumber = GetRailsPatternNumber(lastRailsPatternNumber);
        lastRailsPatternNumber = currentRailsPatternNumber;
        var railsPattern = Instantiate(_biomeData.RailsPrefabs[currentRailsPatternNumber], floor.transform)
            .GetComponent<RailsPattern>();
        foreach (var point in railsPattern.StaticEnemiesTeleportationPoints)
            _staticEnemyTeleportationPoints.Add(point);

        if (_lastRail != null)
            _lastRail.NextRail = railsPattern.FirstRail;

        if (numberOfBiome == 0)
            LevelSpawner.Instance.CurrentBiomeStartRail = railsPattern.FirstRail;

        _lastRail = railsPattern.LastRail;

        if (numberOfBiome == 0 && LevelSpawner.Instance.CurrentBiomeFinishRail != null)
        {
            if(_isSimplePhase)
                LevelSpawner.Instance.CurrentBiomeFinishRail.NextRail.NextRail = railsPattern.FirstRail;
            else
                LevelSpawner.Instance.CurrentBiomeFinishRail.NextRail= railsPattern.FirstRail;
        }
            

        EnemiesManager.Instance.SpawnDestroyableObjects(railsPattern.DestroyableObjectsSpawnPoints);

        return lastRailsPatternNumber;
    }

    private int GetRailsPatternNumber(int lastRailsPatternNumber)
    {
        int countOfRailsPatterns = _biomeData.RailsPrefabs.Count;
        int currentRailsPatternNumber = Random.Range(0, countOfRailsPatterns);
        while (currentRailsPatternNumber == lastRailsPatternNumber)
            currentRailsPatternNumber = Random.Range(0, countOfRailsPatterns);

        return currentRailsPatternNumber;
    }

    private Vector3 CreateEndWall(Vector3 endWallPosition)
    {
        _endWall = Instantiate(_endWallPrefab, transform).GetComponent<EndWall>();
        _endWall.Init(endWallPosition, _biomeData.EndWallSprite);
        _lastRail.NextRail = _endWall.FinishRail;
        LevelSpawner.Instance.CurrentBiomeFinishRail = _endWall.FinishRail;
        return _endWall.NextLevelSpawnPoint.position;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Trolley trolley))
        {
            if (!_isSimplePhase)
                LevelSpawner.Instance.SpawnBiome();
        }
    }
}