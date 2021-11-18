using System.Collections.Generic;
using UnityEngine;

public class RailsPattern : MonoBehaviour
{
    [Header("Enemies Spawn Points")]
    [SerializeField] private List<Transform> _flyingEnemiesSpawnPoints;
    [SerializeField] private List<Transform> _staticEnemiesTeleportationPoints;
    [SerializeField] private List<Transform> _destroyableObjectsSpawnPoints;

    [Header("Rails")]
    [SerializeField] private Rail _firstRail;
    [SerializeField] private Rail _lastRail;

    public List<Transform> FlyingEnemiesSpawnPoints => _flyingEnemiesSpawnPoints;

    public List<Transform> StaticEnemiesTeleportationPoints => _staticEnemiesTeleportationPoints;

    public List<Transform> DestroyableObjectsSpawnPoints => _destroyableObjectsSpawnPoints;

    public Rail FirstRail => _firstRail;

    public Rail LastRail => _lastRail;
}
