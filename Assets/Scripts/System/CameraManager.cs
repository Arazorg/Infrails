using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private CinemachineTransposer _cinemachineTransposer;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private float _needOrtographicSize = 44f;
    private float _resizingSpeed;
    private float _shakeElapsedTime;

    public void Init()
    {
        _cinemachineTransposer 
            = _cinemachineVirtualCamera.AddCinemachineComponent<CinemachineTransposer>();
        _cinemachineBasicMultiChannelPerlin 
            = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetCameraParams(Transform target, float ortographicSize, Vector3 offset, float resizingSpeed = 5f)
    {
        _cinemachineVirtualCamera.m_LookAt = target;
        _cinemachineVirtualCamera.m_Follow = target;
        _needOrtographicSize = ortographicSize;
        _cinemachineTransposer.m_FollowOffset = offset;
        _resizingSpeed = resizingSpeed;
    }

    public void ShakeCameraOnce(float shakeDuration, float intensity)
    {
        if(SettingsInfo.Instance.IsCameraShake)
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            _shakeElapsedTime = shakeDuration;
        }
    }

    private void Update()
    {
        ChangeOrtographicSize();
        Shaking();
    }

    private void ChangeOrtographicSize()
    {
        float currentOrtographicSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float tempOrtograpichSize = Mathf.Lerp(currentOrtographicSize, _needOrtographicSize, _resizingSpeed * Time.deltaTime);
        _cinemachineVirtualCamera.m_Lens.OrthographicSize = tempOrtograpichSize;
    }

    private void Shaking()
    {
        if(_shakeElapsedTime > 0)
        {
            _shakeElapsedTime -= Time.deltaTime;
            if (_shakeElapsedTime <= 0)
            {
                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }  
    }
}
