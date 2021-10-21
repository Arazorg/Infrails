using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private GameObject _endWallPrefab;
    [SerializeField] private Transform _levelFloorsParentObject;

    private BiomeData _biomeData;
    private List<RailsPattern> _currentRailsPatterns = new List<RailsPattern>();
    private Rail _lastRail;
    private GameObject _endWall;

    public Vector3 Init(BiomeData data, BiomeData nextBiomeData)
    {
        _biomeData = data;
        int minLengthOfLevel = 4;
        int maxLenghtOfLevel = 7;
        Vector3 nextLevelSpawnPosition = CreateEnvironment(Random.Range(minLengthOfLevel, maxLenghtOfLevel));
        SetNextBiomeData(nextBiomeData);
        SetBiomeLightsState(true);
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
        Destroy(gameObject, 5f);
    }

    private Vector3 CreateEnvironment(int lengthOfLevel)
    {
        Vector3 nextFloorSpawnPosition = transform.position;
        int lastRailsPatternNumber = Random.Range(0, _biomeData.RailsPrefabs.Count);
        _currentRailsPatterns.Clear();

        for (int i = 0; i < lengthOfLevel; i++)
        {
            GameObject floor = Instantiate(_floorPrefab, nextFloorSpawnPosition, Quaternion.identity, _levelFloorsParentObject);
            floor.GetComponent<SpriteRenderer>().sprite = _biomeData.FloorSprite;
            nextFloorSpawnPosition += floor.GetComponent<Floor>().NextFloorSpawnPoint.localPosition;

            int currentRailsPatternNumber = GetRailsPatternNumber(lastRailsPatternNumber);
            lastRailsPatternNumber = currentRailsPatternNumber;
            GameObject railsPattern = Instantiate(_biomeData.RailsPrefabs[currentRailsPatternNumber], floor.transform);
            
            _currentRailsPatterns.Add(railsPattern.GetComponent<RailsPattern>());
            if (_lastRail != null)
            {
                var railsPatternScript = railsPattern.GetComponent<RailsPattern>();
                _lastRail.NextRail = railsPatternScript.FirstRail;     
            }

            if (i == 0)
                LevelSpawner.Instance.CurrentBiomeStartRail = railsPattern.GetComponent<RailsPattern>().FirstRail;
                
            _lastRail = railsPattern.GetComponent<RailsPattern>().LastRail;

            if (i == 0 && LevelSpawner.Instance.CurrentBiomeFinishRail != null)
                LevelSpawner.Instance.CurrentBiomeFinishRail.NextRail = railsPattern.GetComponent<RailsPattern>().FirstRail;

            EnemiesManager.Instance.SpawnDestroyableObjects(railsPattern.GetComponent<RailsPattern>().DestroyableObjectsSpawnPoints);
        }

        Vector3 nextLevelSpawnPosition = CreateEndWall(nextFloorSpawnPosition);
        return nextLevelSpawnPosition;
    }

    private int GetRailsPatternNumber(int lastRailsPatternNumber)
    {
        int countOfRailsPatterns = _biomeData.RailsPrefabs.Count;
        int currentRailsPatternNumber = Random.Range(0, countOfRailsPatterns);
        while (currentRailsPatternNumber == lastRailsPatternNumber)
            currentRailsPatternNumber = Random.Range(0, countOfRailsPatterns);

        return currentRailsPatternNumber;
    }

    private Vector3 CreateEndWall(Vector3 nextFloorSpawnPosition)
    {
        Vector3 endWallOffset = new Vector3(0, 0.25f, 0);
        Vector3 endWallPosition = nextFloorSpawnPosition + endWallOffset;

        _endWall = Instantiate(_endWallPrefab, transform);
        _endWall.transform.position = endWallPosition;
        _endWall.GetComponent<SpriteRenderer>().sprite = _biomeData.EndWallSprite;

        var endWallFinishRail = _endWall.GetComponent<EndWall>().FinishRail;
        _lastRail.NextRail = endWallFinishRail;
        LevelSpawner.Instance.CurrentBiomeFinishRail = endWallFinishRail.GetComponent<Rail>();

        return _endWall.GetComponent<EndWall>().NextLevelSpawnPoint.position;
    }
}
