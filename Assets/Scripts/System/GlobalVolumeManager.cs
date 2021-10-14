using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeManager : MonoBehaviour
{
    public static GlobalVolumeManager Instance;

    private Volume _volume;

    public void SetDepthOfFieldState(bool isState)
    {
        /*
        if (!_volume)
            _volume = GetComponent<Volume>();
            
        if (_volume.profile.TryGet(out DepthOfField dofComponent))
        {
            if (isState)
                dofComponent.active = true;           
            else
                dofComponent.active = false;   
        }
        */
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
