using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [SerializeField] private GameObject _damageEffect;
    [SerializeField] private EffectData _deathEffectData;
    [SerializeField] private EffectData _rebornEffectData;
    [SerializeField] private EffectData _dizzinesEffectData;
    [SerializeField] private EffectData _healingEffectData;

    private Character _character;
    private GameObject _dizzinesEffect;
    private GameObject _healingEffect;

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

    private void Start()
    {
        _character = GetComponent<Character>();
        SubscribeOnEvent();
    }

    private void SubscribeOnEvent()
    {
        _character.OnCharacterDeath += SpawnDeathEffect;
        _character.OnHealthChanged += SpawnHealingEffect;
        _character.OnCharacterReborn += SpawnRebornEffect;
    }

    private void SpawnDeathEffect()
    {
        SetCharacterVisibility(false);
        var effect = SpawnEffect(_deathEffectData);
        var settings = effect.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(_character.CharacterData.UnitColor);
    }

    private void SpawnRebornEffect()
    {
        SetCharacterVisibility(true);
        var effect = SpawnEffect(_rebornEffectData);
        effect.GetComponent<Animator>().runtimeAnimatorController 
            = _character.CharacterData.TeleportationAnimatorController;
    }

    private void SetCharacterVisibility(bool isState)
    {
        GetComponent<BoxCollider2D>().enabled = isState;
        GetComponent<SpriteRenderer>().enabled = isState;
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = isState;
    }

    private void SpawnHealingEffect(int health, int value)
    {
        if(_healingEffect == null && value > 0)
        {
            var effect = SpawnEffect(_healingEffectData);
            Destroy(effect, _healingEffectData.DestroyDelay);            
        }       
        else
        {
            var particleSettings = _damageEffect.GetComponent<ParticleSystem>().main;
            particleSettings.startColor = _character.CharacterData.UnitColor;
            GameObject explosion = Instantiate(_damageEffect, transform.position, Quaternion.identity);
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        }
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
