using Cinemachine;
using System;
using System.Collections;
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
        _cinemachineTransposer.m_FollowOffset = offset;
        StartCoroutine(SetSize(ortographicSize));
    }

    public void SetLobbyCameraParams(Transform target, float ortographicSize, Vector3 offset)
    {
        StopAllCoroutines();
        Vector3 finishPosition = target.position + offset;
        finishPosition.z = -10;
        _cinemachineVirtualCamera.transform.position = finishPosition;
        //_cinemachineVirtualCamera.m_Lens.OrthographicSize = ortographicSize;
        StartCoroutine(SetSize(ortographicSize));
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
            ShakeCameraOnce(duration, intensity);
    }

    private void DeathShaking()
    {
        float duration = .35f;
        float intensity = 25f;
        Camera.main.GetComponent<CameraManager>().ShakeCameraOnce(duration, intensity);
    }

    private IEnumerator SetSize(float ortographicSize)
    {
        float duration = 0.33f;
        float time = 0;
        float startSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;

        while (time < duration)
        {
            _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, ortographicSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}