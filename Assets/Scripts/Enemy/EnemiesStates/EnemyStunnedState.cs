public class EnemyStunnedState : BaseEnemyState
{
    public EnemyStunnedState(IEnemyStateSwitcher stateSwitcher)
        : base(stateSwitcher) { }

    public override void Start() { }

    public override void Idle()
    {
        StateSwitcher.SwitchState<EnemyIdleState>();
    }

    public override void Move()
    {
        StateSwitcher.SwitchState<EnemyMovementState>();
    }

    public override void Attack()
    {
        StateSwitcher.SwitchState<EnemyAttackState>();
    }

    public override void Stun() { }

    public override void Stop() { }
}
