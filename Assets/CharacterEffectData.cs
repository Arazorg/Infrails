using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Standart Character Effect", fileName = "New Character Effect")]
public class CharacterEffectData : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private AudioClip _effectAudioClip;
    [SerializeField] private Color _effectColor;
    [SerializeField] private Vector3 _effectOffset;
    [SerializeField] private float _destroyDelay;

    public GameObject Prefab => _prefab;

    public AudioClip EffectAudioClip => _effectAudioClip;

    public Color EffectColor => _effectColor;

    public Vector3 EffectOffset => _effectOffset;

    public float DestroyDelay => _destroyDelay;
}
