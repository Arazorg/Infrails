using UnityEngine;

public class ManeCrystal : Enemy
{
    private const float SwayDuration = 0.5f;

    [SerializeField] private Transform _shadowTransform;

    private Character _character;
    private Vector3 _startPosition;
    private Vector3 _shadowStartPosition;
    private Vector3 _startOffset;
    private Vector3 _finishOffset;
    private float _swayTime = float.MaxValue;
    private float _offsetFactorY = 0.8f;

    public override void Init(EnemyData data, Transform spawnPoint, GameObject target)
    {
        Data = data;   
        OnInit();
        if (target == null)
            EnemiesManager.Instance.OnTargetInit += GetCharacter;
        else
            _character = target.GetComponent<Character>();
    }

    protected override void Death()
    {
        _character.SetWeaponElement(Data.EnemyElement);
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

    private void GetCharacter(GameObject target)
    {
        EnemiesManager.Instance.OnTargetInit -= GetCharacter;
        _character = target.GetComponent<Character>();
    }
}
