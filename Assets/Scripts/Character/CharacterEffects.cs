using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [SerializeField] private EffectData _deathEffectData;
    [SerializeField] private EffectData _rebornEffectData;
    [SerializeField] private EffectData _dizzinesEffectData;

    private GameObject _dizzinesEffect;

    public void SpawnDeathEffect(Color color)
    {
        var effect = SpawnEffect(_deathEffectData);
        var settings = effect.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);
    }

    public void SpawnDizzinesEffect()
    {
        if (_dizzinesEffect != null)
            DestroyDizzinesEffect();

        _dizzinesEffect = SpawnEffect(_dizzinesEffectData);
    }

    public void DestroyDizzinesEffect()
    {
        Destroy(_dizzinesEffect);
    }

    public void SpawnRebornEffect(RuntimeAnimatorController animator)
    {
        var effect = SpawnEffect(_rebornEffectData);
        effect.GetComponent<Animator>().runtimeAnimatorController = animator;
    }

    public void SetCharacterVisibility(bool isState)
    {
        GetComponent<BoxCollider2D>().enabled = isState;
        GetComponent<SpriteRenderer>().enabled = isState;
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = isState;
    }

    private GameObject SpawnEffect(EffectData data)
    {
        if (data.EffectAudioClip != null)
            AudioManager.Instance.PlayEffect(data.EffectAudioClip);

        var effectPosition = transform.position + data.EffectOffset;
        var effect = Instantiate(data.Prefab, effectPosition, Quaternion.identity, transform);
        effect.GetComponent<SpriteRenderer>().color = data.EffectColor;
        if (data.DestroyDelay != 0)
            Destroy(effect, data.DestroyDelay);

        return effect;
    }
}
