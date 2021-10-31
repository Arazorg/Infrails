using UnityEngine;

public class TrolleyMovement : MonoBehaviour
{
    private const string TurnRightAnimationKey = "TurnRight";
    private const string TurnLeftAnimationKey = "TurnLeft";
    private const float DistanceForGetNewPosition = 0.01f;

    private Animator _animator;
    private Rail _nextRail;
    private Transform _previousRail;
    private int _speed;
    private int _startSpeed;
    private int _speedDebuff;
    private bool _isMove;

    public int Speed { set => _speed = value; }

    public Rail NextRail { get => _nextRail; set => _nextRail = value; }

    public bool IsMove { get => _isMove; set => _isMove = value; }

    public void Init(int speed, int speedDebuff)
    {
        _speed = speed;
        _startSpeed = speed;
        _speedDebuff = speedDebuff;
    }

    public void StartSpeedDebaff()
    {
        _speed -= _speedDebuff;
    }

    public void StopSpeedDebaff()
    {
        _speed = _startSpeed;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _isMove = true;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (_isMove)
        {
            if (_nextRail != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, _nextRail.transform.position, _speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, _nextRail.transform.position) < DistanceForGetNewPosition)
                {
                    if (_nextRail != null && _previousRail != null)
                        Turn(_previousRail, _nextRail.NextRail.RailTransform);

                    transform.position = _nextRail.RailTransform.position;
                    _previousRail = _nextRail.RailTransform;
                    _nextRail = _nextRail.NextRail;
                }
            }
        }
    }

    private void Turn(Transform previousRail, Transform nextRail)
    {
        if ((transform.position.x > previousRail.position.x && transform.position.y > nextRail.position.y)
                || (transform.position.y > previousRail.position.y && transform.position.x > nextRail.position.x))
            _animator.Play(TurnLeftAnimationKey);
        else if ((transform.position.x > previousRail.position.x && transform.position.y < nextRail.position.y)
                    || (transform.position.x > nextRail.position.x && transform.position.y < previousRail.position.y))
            _animator.Play(TurnLeftAnimationKey);
        else if ((transform.position.y > previousRail.position.y && transform.position.x < nextRail.position.x)
                || (transform.position.x < previousRail.position.x && transform.position.y > nextRail.position.y))
            _animator.Play(TurnRightAnimationKey);
        else if ((transform.position.x < previousRail.position.x && transform.position.y < nextRail.position.y)
                    || (transform.position.x < nextRail.position.x && transform.position.y < previousRail.position.y))
            _animator.Play(TurnRightAnimationKey);
    }
}
