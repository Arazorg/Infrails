using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EndWall : MonoBehaviour
{
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer _endWallFloor;
    [SerializeField] private SpriteRenderer _plateSpriteRenderer;

    [SerializeField] private Transform _nextLevelSpawnPoint;
    [SerializeField] private Rail _finishRail;

    private Light2D _nextLevelLight;
    private float _lightOnOffDuration = 2f;
    private float _neededLigthIntensity = 0f;
    private float _timeElapsedLightIntensity = float.PositiveInfinity;

    public Transform NextLevelSpawnPoint => _nextLevelSpawnPoint;

    public Rail FinishRail => _finishRail;

    public void SetEndWallEnvironment(BiomeData nextLevelData, Sprite endWallFloorSprite)
    {
        _nextLevelLight = GetComponentInChildren<Light2D>();
        _nextLevelLight.color = nextLevelData.BiomeColor;
        _plateSpriteRenderer.sprite = nextLevelData.PlateSprite;
        _endWallFloor.sprite = endWallFloorSprite;
    }

    public void SetNeedLightsIntensity(float neededGlobalLigthIntensity)
    {
        _timeElapsedLightIntensity = 0;
        _neededLigthIntensity = neededGlobalLigthIntensity;

        float disableDuration = 2f;
        float enableDuration = 1f;
        if (neededGlobalLigthIntensity == 0)
            _lightOnOffDuration = disableDuration;
        else        
            _lightOnOffDuration = enableDuration;
    }

    private void Update()
    {
        SetLightsIntensity();
    }

    private void SetLightsIntensity()
    {
        if (_timeElapsedLightIntensity < _lightOnOffDuration)
        {
            float intensity = Mathf.Lerp(_nextLevelLight.intensity, _neededLigthIntensity, _timeElapsedLightIntensity / _lightOnOffDuration);
            _nextLevelLight.intensity = intensity;
            _timeElapsedLightIntensity += Time.deltaTime;
        }
    }
}
