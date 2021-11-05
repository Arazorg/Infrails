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
    private Coroutine _destroyByLaserCoroutine;
    private float _timeToDestroyByLaser = 0;

    public enum KitType
    {
        FirstAidKit,
        RepairKit
    }

    public Transform LaserAttackPoint => _laserAttackPoint;

    public bool IsVisible => IsGetDamage;

    public override void Init(EnemyData data, Transform spawnPoint, Character character)
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

    protected override void Death(bool isDeathWithEffect)
    {
        if (isDeathWithEffect)
            ChooseActionByType();

        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void TryGetCharacter(Character character)
    {
        if (character == null)
            EnemiesManager.Instance.OnCharacterAvailable += OnCharacterAvailable;
        else
            Character = character;
    }

    private void OnCharacterAvailable(Character character)
    {
        EnemiesManager.Instance.OnCharacterAvailable -= OnCharacterAvailable;
        Character = character;
    }

    private void ChooseActionByType()
    {
        if (_type == KitType.FirstAidKit)
            Character.Heal(HealValue);
        else if (_type == KitType.RepairKit)
            Character.RepairArmor(RepairValue);
    }

    private IEnumerator DestroyByLaser()
    {
        float timeToDestroyByLaser = 1.75f;
        while (_timeToDestroyByLaser < timeToDestroyByLaser)
        {
            _timeToDestroyByLaser += Time.deltaTime;
            yield return null;
        }

        Death(GameConstants.DeathWithoutEffect);
    }

    private void SpawnDestructionEffect()
    {
        _destructionEffect = Instantiate(_destructionEffectPrefab, _laserAttackPoint);
        var settings = _destructionEffect.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(Data.UnitColor);
    }
}
