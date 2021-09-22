using System;
using UnityEngine;

[Serializable]
public class Audio
{
    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool isLoop;

    [HideInInspector]
    public AudioSource audioSource;
}
