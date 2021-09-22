using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EndWall : MonoBehaviour, IUpdateable
{
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
        UpdateManager.Instance.Register(this);
    }

    public void SetNeedLightsIntensity(float neededGlobalLigthIntensity)
    {
        _timeElapsedLightIntensity = 0;
        _neededLigthIntensity = neededGlobalLigthIntensity;

        if (neededGlobalLigthIntensity == 0)
        {
            _lightOnOffDuration = 2f;
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
