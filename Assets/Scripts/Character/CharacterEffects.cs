using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [SerializeField] private CharacterEffectData _deathEffectData;
    [SerializeField] private CharacterEffectData _rebornEffectData;
    [SerializeField] private CharacterEffectData _healingEffectData;
    [SerializeField] private CharacterEffectData _repairArmorEffectData;

    public void SpawnDeathEffect(Color color)
    {
        var effect = SpawnEffect(_deathEffectData);
        var settings = effect.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);
    }

    public void SpawnRebornEffect(RuntimeAnimatorController animator)
    {
        var effect = SpawnEffect(_rebornEffectData);
        effect.GetComponent<Animator>().runtimeAnimatorController = animator;
    }

    public void SpawnHealingEffect()
    {
        SpawnEffect(_healingEffectData);
    }

    public void SpawnRepairArmorEffect()
    {
        SpawnEffect(_repairArmorEffectData);
    }

    public void SetCharacterVisibility(bool isState)
    {
        GetComponent<BoxCollider2D>().enabled = isState;
        GetComponent<SpriteRenderer>().enabled = isState;
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = isState;
    }

    private GameObject SpawnEffect(CharacterEffectData data)
    {
        AudioManager.Instance.PlayEffect(data.EffectAudioClip);
        var effectPosition = transform.position + data.EffectOffset;
        var effect = Instantiate(data.Prefab, effectPosition, Quaternion.identity, transform);
        effect.GetComponent<SpriteRenderer>().color = data.EffectColor;
        Destroy(effect, data.DestroyDelay);
        return effect;
    }
}
