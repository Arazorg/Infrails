using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform enemyShadowTransform;

    private Transform _target;
    private Transform _spawnPoint;
    private EnemyAttack _enemyAttack;
    private Vector3 _needPosition;
    private Quaternion _needQuaternion;
    private Quaternion _startShadowQuaternion;
    private bool _isFacingRight;
    private bool _isChase;

    public bool IsChase
    {
        set
        {
            _isChase = value;
            if (_isChase)
                StartCoroutine(GetNextPoint());
            else
                StopCoroutine(GetNextPoint());
        }
    }

    public void Init(Transform spawnPoint, Transform target)
    {
        _spawnPoint = spawnPoint;
        _target = target;
        _isChase = true;
        StartCoroutine(GetNextPoint());
        LevelSpawner.Instance.OnBiomeSpawned += MoveToSpawnPoint;
    }

    private void Start()
    {
        _startShadowQuaternion = enemyShadowTransform.rotation;
    }

    private void Update()
    {
        FlipToTarget();
        MoveToTarget();
        RotateToTarget();
        enemyShadowTransform.rotation = _startShadowQuaternion;
    }

    private void MoveToTarget()
    {
        float minDistanceX = 10f;
        if (_target != null && _isChase)
        {
            if (System.Math.Abs(_target.position.x - transform.position.x) < minDistanceX)
                GetNextPoint();
        }

        transform.position = Vector3.Lerp(transform.position, _needPosition, Time.fixedDeltaTime / 0.66f);
    }

    private void MoveToSpawnPoint()
    {
        StopCoroutine(GetNextPoint());
        _isChase = false;
        _target = _spawnPoint;
        _needPosition = _spawnPoint.position;
        Destroy(gameObject, 2f);
    }

    private void SetStateOfAttack()
    {
        float minDistance = 2f;
        float maxDistance = 7f;

        var distance = transform.position.y - _target.position.y;
        if (minDistance < distance && distance < maxDistance)
            GetComponent<EnemyAttack>().IsAttack = true;
        else
            GetComponent<EnemyAttack>().IsAttack = false;
    }

    private IEnumerator GetNextPoint()
    {
        float minDistanceX = 15f;

        if (_target != null)
        {
            while (true)
            {
                if (_target.position.x > _spawnPoint.position.x)
                {
                    _needPosition.x = _target.position.x + Random.Range(-5f, 0f) + -minDistanceX;
                    _needPosition.y = _target.position.y + Random.Range(-3f, 30f);
                }
                else if (_target.position.x < _spawnPoint.position.x)
                {
                    _needPosition.x = _target.position.x + Random.Range(0f, 5f) + minDistanceX;
                    _needPosition.y = _target.position.y + Random.Range(-3f, 30f);
                }

                yield return new WaitForSeconds(0.85f + Random.Range(-0.1f, 0.1f));

                if (Random.Range(0, 100) < 25f)
                    _spawnPoint.position = new Vector3(_spawnPoint.position.x * -1, _spawnPoint.position.y, _spawnPoint.position.z);
            }
        }

        yield return null;
    }

    private void RotateToTarget()
    {
        if (_target != null)
        {
            if (transform.position.y < _target.position.y)
                _needQuaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(-15, -5)));
            else
                _needQuaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(5, 15)));

            transform.rotation = Quaternion.Lerp(transform.rotation, _needQuaternion, Time.deltaTime / 0.4f);
        }
    }

    private void FlipToTarget()
    {
        if (_target != null)
        {
            if (_target.position.x < transform.position.x && _isFacingRight)
                Flip();
            else if (_target.position.x > transform.position.x && !_isFacingRight)
                Flip();
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDestroy()
    {
        LevelSpawner.Instance.OnBiomeSpawned -= MoveToSpawnPoint;
    }
}
