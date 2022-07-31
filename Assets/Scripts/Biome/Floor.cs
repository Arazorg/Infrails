using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Floor : MonoBehaviour
{
    [SerializeField] private Transform _nextFloorSpawnPoint;
    [SerializeField] private Light2D _floorLight;

    private float _lightOnOffDuration = 2f;
    private float _startGlobalLightIntensity;
    private float _neededGlobalLigthIntensity;
    private float _timeElapsedLightIntensity = float.PositiveInfinity;

    public Transform NextFloorSpawnPoint => _nextFloorSpawnPoint;

    public void SetNeedLightsIntensity(float neededGlobalLigthIntensity)
    {
        float disableLightDuration = 1f;
        float enableLightDuration = 2.15f;

        _timeElapsedLightIntensity = 0;
        _neededGlobalLigthIntensity = neededGlobalLigthIntensity;

        if (neededGlobalLigthIntensity == 0)
        {
            _startGlobalLightIntensity = 1;
            _lightOnOffDuration = disableLightDuration;
        }
        else
        {
            _startGlobalLightIntensity = 0;
            _lightOnOffDuration = enableLightDuration;
        }
            
    }

    private void Update()
    {
        SetLightsIntensity();
    }

    private void SetLightsIntensity()
    {
        if (_timeElapsedLightIntensity < _lightOnOffDuration)
        {
            float t = _timeElapsedLightIntensity / _lightOnOffDuration;
            _floorLight.intensity = Mathf.Lerp(_startGlobalLightIntensity, _neededGlobalLigthIntensity, t);
            _timeElapsedLightIntensity += Time.deltaTime;
        }
    }
}
