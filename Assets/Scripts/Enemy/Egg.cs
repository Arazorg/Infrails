using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Enemy, IEnemyLaserTarget
{
    private const float SwayDuration = 0.5f;

    [SerializeField] private Transform _shadowTransform;
    [SerializeField] private Transform _laserAttackPoint;

    private Coroutine _destroyByLaserCoroutine;
    private float _swayTime = float.MaxValue;
    private float _angleFactor = 1f;
    private float _timeToDestroyByLaser = 0; 

    public Transform LaserAttackPoint => _laserAttackPoint;

    public bool IsVisible => IsGetDamage;

    public override void Init(EnemyData data, Transform spawnPoint, GameObject target)
    {
        Data = data;
        Target = target;
        OnInit();
    }

    public void StartLaserInteraction()
    {
        _destroyByLaserCoroutine = StartCoroutine(DestroyByLaser());
    }

    public void StopLaserInteraction()
    {
        StopCoroutine(_destroyByLaserCoroutine);
    }

    protected override void Death()
    {
        EnemiesManager.Instance.SpawnFlyingEnemies(new List<Transform>() { transform });
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void Start()
    {
        SetAngleFactor();
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
            float angle = 10;
            float currentAngle = Mathf.Lerp(angle * _angleFactor, angle * -_angleFactor, _swayTime / SwayDuration);
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            _shadowTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, -currentAngle));
            _swayTime += Time.deltaTime;
        }
        else
        {
            _angleFactor *= -1;
            _swayTime = 0;
        }
    }

    private void SetAngleFactor()
    {
        if (Random.Range(0, 2) == 0)
            _angleFactor *= -1;
    }

    private IEnumerator DestroyByLaser()
    {
        float timeToDestroyByLaser = 1.75f;
        while (_timeToDestroyByLaser < timeToDestroyByLaser)
        {
            _timeToDestroyByLaser += Time.deltaTime;
            yield return null;  
        }          

        Death();
    }
}
