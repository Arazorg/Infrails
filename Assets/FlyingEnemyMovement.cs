using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    private const int Speed = 37;

    [SerializeField] private Transform enemyShadowTransform;

    private Transform _target;
    private Vector3 _spawnPointPosition;
    private Vector3 _needPosition;
    private Quaternion _startShadowQuaternion;
    private bool _isMove;

    public delegate void ReachedNextPoint();

    public event ReachedNextPoint OnReachedNextPoint;

    public void Init(Vector3 spawnPointPosition, Transform target)
    {
        _startShadowQuaternion = enemyShadowTransform.rotation;
        _spawnPointPosition = spawnPointPosition;
        _target = target;
    }

    public void StartMove()
    {
        SetNextPoint();
        _isMove = true;
    }

    private void Update()
    {
        FixShadowRotation();
        MoveToNextPoint();
    }

    private void FixShadowRotation()
    {
        enemyShadowTransform.rotation = _startShadowQuaternion;
    }


    private void MoveToNextPoint()
    {
        if (_isMove)
            transform.position = Vector2.MoveTowards(transform.position, _needPosition, Speed * Time.deltaTime);

        float distanceToSetNextPoint = 1f;
        if (Vector3.Distance(transform.position, _needPosition) < distanceToSetNextPoint)
        {
            _needPosition = Vector3.zero;
            _isMove = false;
            OnReachedNextPoint?.Invoke();
        }
    }

    private void SetNextPoint()
    {
        float minDistanceX = 12.5f;
        float minOffsetX = 7.5f;
        float maxOffsetX = 10;
        float minOffsetY = 15;
        float maxOffsetY = 50;

        if (_target != null)
        {
            if (_target.position.x > _spawnPointPosition.x)
            {
                _needPosition.x = _target.position.x + Random.Range(-maxOffsetX, -minOffsetX) - minDistanceX;
                _needPosition.y = _target.position.y + Random.Range(-minOffsetY, maxOffsetY);
            }
            else if (_target.position.x < _spawnPointPosition.x)
            {
                _needPosition.x = _target.position.x + Random.Range(minOffsetX, maxOffsetX) + minDistanceX;
                _needPosition.y = _target.position.y + Random.Range(-minOffsetY, maxOffsetY);
            }

            if (Random.value < 0.25f)
                _spawnPointPosition = new Vector2(_spawnPointPosition.x * -1, _spawnPointPosition.y);
        }

    }
}
