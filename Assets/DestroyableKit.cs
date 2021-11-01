using System.Collections;
using UnityEngine;

public class DestroyableKit : Enemy, IEnemyLaserTarget
{
    private const int HealValue = 5;
    private const int RepairValue = 3;

    [SerializeField] private GameObject _destructionEffectPrefab;
    [SerializeField] private Transform _laserAttackPoint;
    [SerializeField] private KitType _type;  

    private GameObject _destructionEffect;
    private Character _characterScript;
    private Coroutine _destroyByLaserCoroutine;
    private float _timeToDestroyByLaser = 0;

    public enum KitType
    {
        FirstAidKit,
        RepairKit
    }

    public Transform LaserAttackPoint => _laserAttackPoint;

    public bool IsVisible => IsGetDamage;

    public override void Init(EnemyData data, Transform spawnPoint, GameObject character)
    {
        Data = data;
        OnInit();
        TryGetCharacter(character);
    }

    public void StartLaserInteraction()
    {
        SpawnDestructionEffect();
        _destroyByLaserCoroutine = StartCoroutine(DestroyByLaser());
    }

    public void StopLaserInteraction()
    {
        Destroy(_destructionEffect);
        StopCoroutine(_destroyByLaserCoroutine);
    }

    protected override void Death()
    {
        ChooseActionByType();
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void TryGetCharacter(GameObject character)
    {
        if (character == null)
        {
            EnemiesManager.Instance.OnCharacterAvailable += OnCharacterAvailable;
        }
        else
        {
            _characterScript = character.GetComponent<Character>();
        }
    }

    private void OnCharacterAvailable(GameObject character)
    {
        EnemiesManager.Instance.OnCharacterAvailable -= OnCharacterAvailable;
        _characterScript = character.GetComponent<Character>();
    }

    private void ChooseActionByType()
    {
        if (_type == KitType.FirstAidKit)
            _characterScript.Heal(HealValue);
        else if (_type == KitType.RepairKit)
            _characterScript.RepairArmor(RepairValue);
    }

    private IEnumerator DestroyByLaser()
    {
        float timeToDestroyByLaser = 1.75f;
        while (_timeToDestroyByLaser < timeToDestroyByLaser)
        {
            _timeToDestroyByLaser += Time.deltaTime;
            yield return null;
        }

        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void SpawnDestructionEffect()
    {
        _destructionEffect = Instantiate(_destructionEffectPrefab, _laserAttackPoint);
        var settings = _destructionEffect.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(Data.UnitColor);
    }
}
