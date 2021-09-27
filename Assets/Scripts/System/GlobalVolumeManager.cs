using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeManager : MonoBehaviour
{
    public static GlobalVolumeManager Instance;

    private Volume _volume;

    public void SetDepthOfFieldState(bool isState)
    {
        float _disableDFFocusDistance = 5;
        float _enableDFFocusDistance = 0.5f;

        if (!_volume)
            _volume = GetComponent<Volume>();
            
        if (_volume.profile.TryGet(out DepthOfField dofComponent))
        {
            if (isState)
                dofComponent.focusDistance.value =_enableDFFocusDistance ;
            else
                dofComponent.focusDistance.value = _disableDFFocusDistance;
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
