using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [SerializeField] private GameObject _teleportaionPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private AudioClip _teleportationEffectClip;

    public void SpawnDeathEffect(Color color)
    {
        AudioManager.Instance.PlayEffect(_deathClip);
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        var settings = explosion.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);
        Destroy(explosion, settings.startLifetimeMultiplier);
    }

    public void SpawnRebornEffect(RuntimeAnimatorController animator)
    {
        AudioManager.Instance.PlayEffect(_teleportationEffectClip);
        Vector3 teleportationEffectOffset = new Vector3(0, 4, 0);
        var teleportationEffect = Instantiate(_teleportaionPrefab, transform.position + teleportationEffectOffset, Quaternion.identity, transform);
        teleportationEffect.GetComponent<Animator>().runtimeAnimatorController = animator;
        Destroy(teleportationEffect, 0.5f);
    }

    public void SetCharacterVisibility(bool isState)
    {
        GetComponent<BoxCollider2D>().enabled = isState;
        GetComponent<SpriteRenderer>().enabled = isState;
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.enabled = isState;
        }
    }
}
