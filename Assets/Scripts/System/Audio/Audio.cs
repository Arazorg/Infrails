using System;
using UnityEngine;

[Serializable]
public class Audio
{
    [SerializeField] private string _name;
    [SerializeField] private AudioClip _clip;

    [Range(0f, 1f)]
    [SerializeField] private float _volume;
    [Range(.1f, 3f)]
    [SerializeField] private float _pitch;

    [SerializeField] private bool _isLoop;
    [SerializeField] private bool _isRandomPitch;

    [HideInInspector]
    [SerializeField] private AudioSource _audioSource;

    public string Name => _name;

    public AudioClip Clip => _clip;

    public float Volume => _volume;

    public float Pitch => _pitch;

    public bool IsLoop => _isLoop;

    public bool IsRandomPitch => _isRandomPitch;

    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
}
