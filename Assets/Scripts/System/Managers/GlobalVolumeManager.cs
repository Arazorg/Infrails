using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeManager : MonoBehaviour
{
    private const float UIRenderScale = 0.25f;
    private const float DefaultRenderScale = 1f;

    public static GlobalVolumeManager Instance;

    [SerializeField] private UniversalRenderPipelineAsset _universalRenderPipelineAsset;
    [SerializeField] private VolumeProfile _UIVolumeProfile;
    [SerializeField] private VolumeProfile _defaultVolumeProfile;
    [SerializeField] private UniversalAdditionalCameraData _universalAdditionalCameraData;

    private Volume _volume;

    private void Start()
    {
        float standartRenderScale = 1f;
        _universalAdditionalCameraData.renderPostProcessing = false;
        _universalRenderPipelineAsset.renderScale = standartRenderScale;
    }

    public void SetVolumeProfile(bool isUIProfile)
    {
        if (!_volume)
            _volume = GetComponent<Volume>();
            
        if(isUIProfile)
        {
            _universalAdditionalCameraData.renderPostProcessing = true;
            _universalRenderPipelineAsset.renderScale = UIRenderScale;
            _volume.profile = _UIVolumeProfile;
        }
        else
        {
            _universalAdditionalCameraData.renderPostProcessing = false;
            _volume.profile = _defaultVolumeProfile;
            _universalRenderPipelineAsset.renderScale = DefaultRenderScale;
        }
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
