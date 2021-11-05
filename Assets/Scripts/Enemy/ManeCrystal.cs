using System.Collections;
using UnityEngine;

public class ManeCrystal : Enemy, IEnemyLaserTarget
{
    private const float SwayDuration = 0.5f;

    [SerializeField] private GameObject _destructionEffectPrefab;
    [SerializeField] private Transform _shadowTransform;
    [SerializeField] private Transform _laserAttackPoint;

    private GameObject _destructionEffect;
    private Coroutine _destroyByLaserCoroutine;
    private Vector3 _startPosition;
    private Vector3 _shadowStartPosition;
    private Vector3 _startOffset;
    private Vector3 _finishOffset;
    private float _swayTime = float.MaxValue;
    private float _offsetFactorY = 0.8f;
    private float _timeToDestroyByLaser = 0;

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
            Character.SetWeaponElement(Data.EnemyElement);

        AudioManager.Instance.PlayEffect(Data.DeathAudioClip);
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void Start()
    {
        _startPosition = transform.position;
        _shadowStartPosition = _shadowTransform.position;
        SetOffsetFactor();
        SetOffsets();
        _swayTime = 0;
    }

    private void Update()
    {
        Sway();
    }

    private void Sway()
    {
        if (_swayTime <= SwayDuration)
        {
            _swayTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPosition + _startOffset, _startPosition + _finishOffset, _swayTime / SwayDuration);
            _shadowTransform.position = _shadowStartPosition;
        }
        else
        {
            _offsetFactorY *= -1;
            SetOffsets();
            _swayTime = 0;
        }
    }

    private void SetOffsetFactor()
    {
        if (Random.Range(0, 2) == 0)
            _offsetFactorY *= -1;
    }

    private void SetOffsets()
    {
        _startOffset = new Vector3(0, _offsetFactorY, 0);
        _finishOffset = new Vector3(0, -_offsetFactorY, 0);
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
