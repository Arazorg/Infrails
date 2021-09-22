﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<Audio> _audioList;
    [SerializeField] private List<Audio> _musicList;

    private AudioSource _musicSource;
    private List<AudioSource> _currentAudioSorces;
    private Audio _currentBackgroundMusic;
    private int _backgroundMusicNumber = -1;

    public void StartAudio()
    {
        StopAllEffects();
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (SettingsInfo.Instance.IsMusic)
        {
            StopMusic();
            _currentBackgroundMusic = GetBackgroundMusic();
            SetMusicSource();
        }
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void PlayEffect(AudioClip clip)
    {
        if (SettingsInfo.Instance.IsEffects)
        {
            Audio currentAudio = _audioList.Where(s => s.Clip == clip).FirstOrDefault();
            if (currentAudio != null)
            {
                currentAudio.audioSource.pitch = currentAudio.pitch;
                currentAudio.audioSource.PlayOneShot(currentAudio.Clip);
            }
            else
            {
                Debug.LogError($"Effect {name} not found!");
            }
        }
    }

    public void StopAllEffects()
    {
        foreach (AudioSource audioSorce in _currentAudioSorces)
        {
            audioSorce.Stop();
        }
    }

    public void NextBackgroundMusic()
    {
        if (SettingsInfo.Instance.IsMusic)
        {
            _backgroundMusicNumber++;
            if (_backgroundMusicNumber >= _musicList.Count)
                _backgroundMusicNumber = 0;

            _currentBackgroundMusic = _musicList[_backgroundMusicNumber];
            SetMusicSource();
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        InitializeSources();
    }

    private void InitializeSources()
    {
        _currentAudioSorces = new List<AudioSource>();
        _musicSource = gameObject.AddComponent<AudioSource>();
        foreach (var audio in _audioList)
        {
            audio.audioSource = gameObject.AddComponent<AudioSource>();
            audio.audioSource.clip = audio.Clip;
            audio.audioSource.volume = audio.volume;
            audio.audioSource.pitch = audio.pitch;
        }
    }

    private void Update()
    {
        if (!_musicSource.isPlaying)
            PlayMusic();
    }

    private void SetMusicSource()
    {
        _musicSource.clip = _currentBackgroundMusic.Clip;
        _musicSource.Play();
    }

    private Audio GetBackgroundMusic()
    {
        if(_musicList.Count != 1)
        {
            int number = Random.Range(0, _musicList.Count);
            while (_backgroundMusicNumber == number)
                number = Random.Range(0, _musicList.Count);

            _backgroundMusicNumber = number;
            return _musicList[_backgroundMusicNumber];
        }

        return _musicList[0];
    }
}
