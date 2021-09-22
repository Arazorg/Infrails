using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Floor : MonoBehaviour, IUpdateable
{
    [SerializeField] private Transform _nextFloorSpawnPoint;
    [SerializeField] private Light2D _floorLight;

    private float _lightOnOffDuration = 2f;
    private float _neededGlobalLigthIntensity = 0f;
    private float _timeElapsedLightIntensity = float.PositiveInfinity;

    public Transform NextFloorSpawnPoint => _nextFloorSpawnPoint;

    public void SetNeedLightsIntensity(float neededGlobalLigthIntensity)
    {
        _timeElapsedLightIntensity = 0;
        _neededGlobalLigthIntensity = neededGlobalLigthIntensity;

        if (neededGlobalLigthIntensity == 0)
        {
            _lightOnOffDuration = 3f;
        }
        else
        {
            _lightOnOffDuration = 1f;
        }
    }

    public void Tick()
    {
        SetLightsIntensity();
    }

    private void Start()
    {
        UpdateManager.Instance.Register(this);
    }

    private void SetLightsIntensity()
    {
        if (_timeElapsedLightIntensity < _lightOnOffDuration)
        {
            _floorLight.intensity = Mathf.Lerp(_floorLight.intensity, _neededGlobalLigthIntensity, _timeElapsedLightIntensity / _lightOnOffDuration);
            _timeElapsedLightIntensity += Time.deltaTime;
        }
    }
}
