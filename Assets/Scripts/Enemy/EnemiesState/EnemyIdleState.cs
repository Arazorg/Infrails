public class EnemyIdleState : BaseEnemyState
{
    public EnemyIdleState(IEnemyStateSwitcher stateSwitcher)
        : base(stateSwitcher) { }

    public override void Start() { }

    public override void Idle() { }

    public override void Attack()
    {
        StateSwitcher.SwitchState<EnemyAttackState>();
    }

    public override void Move()
    {
        StateSwitcher.SwitchState<EnemyMovementState>();
    }

    public override void Stop() { }
}
