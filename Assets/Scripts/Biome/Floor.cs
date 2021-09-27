using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Floor : MonoBehaviour
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
            _lightOnOffDuration = 3f;
        else
            _lightOnOffDuration = 1f;
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
            _floorLight.intensity = Mathf.Lerp(_floorLight.intensity, _neededGlobalLigthIntensity, t);
            _timeElapsedLightIntensity += Time.deltaTime;
        }
    }
}
