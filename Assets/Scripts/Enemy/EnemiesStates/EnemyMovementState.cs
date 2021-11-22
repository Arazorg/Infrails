public class EnemyMovementState : BaseEnemyState
{
    private IMovableEnemy _movableEnemy;

    public EnemyMovementState(IEnemyStateSwitcher stateSwitcher,
        IMovableEnemy movableEnemy)
        : base(stateSwitcher)
    {
        _movableEnemy = movableEnemy;
    }

    public override void Start()
    {
        _movableEnemy.StartMove();
    }

    public override void Idle() 
    {
        StateSwitcher.SwitchState<EnemyIdleState>();
    }

    public override void Move()
    {
        _movableEnemy.StartMove();
    }

    public override void Attack()
    {
        StateSwitcher.SwitchState<EnemyAttackState>();
    }

    public override void Stun()
    {
        StateSwitcher.SwitchState<EnemyStunnedState>();
    }

    public override void Stop() 
    {
        _movableEnemy.StopMove();
    }
}
