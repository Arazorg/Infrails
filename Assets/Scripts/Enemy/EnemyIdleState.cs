public class EnemyIdleState : BaseEnemyState
{
    private StaticEnemy _staticEnemy;

    public EnemyIdleState(IEnemyStateSwitcher stateSwitcher, StaticEnemy staticEnemy)
        : base(stateSwitcher)
    {
        _staticEnemy = staticEnemy;
    }

    public override void Start() { }

    public override void Attack()
    {
        StateSwitcher.SwitchState<EnemyAttackState>();
    }

    public override void Move()
    {
        StateSwitcher.SwitchState<EnemyMovementState>();
    }

    public override void Transform() { }

    public override void Stop() { }
}
