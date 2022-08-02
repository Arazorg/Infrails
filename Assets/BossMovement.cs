using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Transform enemyShadowTransform;

    private Transform _target;
    private Vector3 _needPosition;
    private Quaternion _startShadowQuaternion;
    private bool _isMove;

    public delegate void ReachedNextPoint();

    public event ReachedNextPoint OnReachedNextPoint;

    public void Init(Transform target)
    {
        _startShadowQuaternion = enemyShadowTransform.rotation;
        _target = target;
    }

    public void StartMove()
    {
        SetNextPoint();
        _isMove = true;
    }

    public void StopMove()
    {
        _isMove = false;
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
        int speed = 20;
        if (_isMove)
            transform.position = Vector2.MoveTowards(transform.position, _needPosition, speed * Time.deltaTime);

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
        float minOffsetX = -12.5f;
        float maxOffsetX = 12.5f;
        float minOffsetY = 45f;
        float maxOffsetY = 55f;

        if (_target != null)
        {
            _needPosition.x = _target.position.x + Random.Range(minOffsetX, maxOffsetX);
            _needPosition.y = _target.position.y + Random.Range(minOffsetY, maxOffsetY);
        }
    }
}