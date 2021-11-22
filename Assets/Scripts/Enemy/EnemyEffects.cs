using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    [SerializeField] private GameObject _deathEffectPrefab;
    [SerializeField] private GameObject _damageEffectPrefab;

    private Enemy _enemy;
    private EnemyData _data;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _data = _enemy.Data;
        _enemy.OnEnemyDamage += SpawnDamageEffect;
        _enemy.OnEnemyDeath += SpawnDeathEffect;
    }

    private void SpawnDeathEffect()
    {
        SpawnEffect(_deathEffectPrefab);
    }

    private void SpawnDamageEffect(int damage)
    {
        SpawnEffect(_damageEffectPrefab);
    }

    private void SpawnEffect(GameObject effectPrefab)
    {
        GameObject effect = Instantiate(effectPrefab, transform.position + (Vector3)_data.Center, Quaternion.identity);
        var mainModule = effect.GetComponent<ParticleSystem>().main;
        mainModule.startColor = _data.UnitColor;
        Destroy(effect, mainModule.startLifetimeMultiplier);
    }
}
