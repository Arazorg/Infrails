using Cinemachine;
using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private CinemachineTransposer _cinemachineTransposer;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    private float _shakeElapsedTime;

    public void Init(Character character = null)
    {
        SubscribeOnCharacterEvent(character);

        _cinemachineTransposer
            = _cinemachineVirtualCamera.AddCinemachineComponent<CinemachineTransposer>();
        _cinemachineBasicMultiChannelPerlin
            = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetCameraParams(Transform target, float ortographicSize, Vector3 offset)
    {
        _cinemachineVirtualCamera.m_LookAt = target;
        _cinemachineVirtualCamera.m_Follow = target;
        _cinemachineVirtualCamera.m_Lens.OrthographicSize = ortographicSize;
        _cinemachineTransposer.m_FollowOffset = offset;
    }

    public void ShakeCameraOnce(float shakeDuration, float intensity)
    {
        if (SettingsInfo.Instance.IsCameraShake)
        {
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            _shakeElapsedTime = shakeDuration;
        }
    }

    private void SubscribeOnCharacterEvent(Character character)
    {
        if (character != null)
        {
            character.OnHealthChanged += DamageShaking;
            character.OnArmorChanged += DamageShaking;
            character.OnCharacterDeath += DeathShaking;
        }
    }

    private void Update()
    {
        Shaking();
    }

    private void Shaking()
    {
        if (_shakeElapsedTime > 0)
        {
            _shakeElapsedTime -= Time.deltaTime;
            if (_shakeElapsedTime <= 0)
            {
                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }

    private void DamageShaking(int health, int value)
    {
        float duration = .1f;
        float intensity = 7f;
        if (value < 0)
            Camera.main.GetComponent<CameraManager>().ShakeCameraOnce(duration, intensity);
    }

    private void DeathShaking()
    {
        float duration = .35f;
        float intensity = 25f;
        Camera.main.GetComponent<CameraManager>().ShakeCameraOnce(duration, intensity);
    }
}
