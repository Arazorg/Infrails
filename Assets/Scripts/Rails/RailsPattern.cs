using System.Collections.Generic;
using UnityEngine;

public class RailsPattern : MonoBehaviour
{
    [SerializeField] private List<Transform> _flyingEnemiesSpawnPoints;
    [SerializeField] private List<Transform> _staticEnemiesSpawnPoints;
    [SerializeField] private List<Transform> _destroyableObjectsSpawnPoints;
    [SerializeField] private Rail _firstRail;
    [SerializeField] private Rail _lastRail;   

    public List<Transform> FlyingEnemiesSpawnPoints
    {
        get { return _flyingEnemiesSpawnPoints; }
    }

    public List<Transform> StaticEnemiesSpawnPoints
    {
        get { return _staticEnemiesSpawnPoints; }
    }

    public List<Transform> DestroyableObjectsSpawnPoints
    {
        get { return _destroyableObjectsSpawnPoints; }
    }

    public Rail FirstRail
    {
        get { return _firstRail; }
    }

    public Rail LastRail
    {
        get { return _lastRail; }
    }
}
