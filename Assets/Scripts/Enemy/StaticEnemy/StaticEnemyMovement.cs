using System.Collections.Generic;
using UnityEngine;

public class StaticEnemyMovement : MonoBehaviour
{
    [SerializeField] private EffectData _teleportationEffectData;

    private StaticEnemyData _staticEnemyData;
    private List<Transform> _teleportationPoints;
    private Transform _currentNextPoint;
    private int _teleportationPointsCounter;

    public delegate void ReachedNextPoint();

    public event ReachedNextPoint OnReachedNextPoint;

    public void Init(StaticEnemyData staticEnemyData, List<Transform> teleportationPoints)
    {
        _staticEnemyData = staticEnemyData;
        _teleportationPoints = teleportationPoints;
        _teleportationPointsCounter = 1;
    }

    public bool Move()
    {
        if(TrySetNextPoint())
        {
            transform.position = _currentNextPoint.position;
            OnReachedNextPoint?.Invoke();
            SpawnTeleportationEffect();
            return true;
        }

        return false;
    }

    private bool TrySetNextPoint()
    {
        if (_teleportationPointsCounter < _teleportationPoints.Count)
        {
            _currentNextPoint = _teleportationPoints[_teleportationPointsCounter];
            _teleportationPointsCounter++;
            return true;
        }

        return false;
    }

    private void SpawnTeleportationEffect()
    {
        var effectPosition = transform.position + _teleportationEffectData.EffectOffset;
        var effect = Instantiate(_teleportationEffectData.Prefab, effectPosition, Quaternion.identity, transform);
        effect.GetComponent<Animator>().runtimeAnimatorController = _staticEnemyData.TeleportationAnimatorController;
        Destroy(effect, _teleportationEffectData.DestroyDelay);
    }
}
