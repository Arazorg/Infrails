public class EnemyAttackState : BaseEnemyState
{
    private IAttackingEnemy _attackingEnemy;

    public EnemyAttackState(IEnemyStateSwitcher stateSwitcher,
        IAttackingEnemy attackingEnemy)
        : base(stateSwitcher)
    {
        _attackingEnemy = attackingEnemy;
    }

    public override void Idle()
    {
        StateSwitcher.SwitchState<EnemyIdleState>();
    }

    public override void Start()
    {
        _attackingEnemy.StartAttack();
    }

    public override void Attack()
    {
        _attackingEnemy.StartAttack();
    }

    public override void Move()
    {
        StateSwitcher.SwitchState<EnemyMovementState>();
    }

    public override void Stop()
    {
        _attackingEnemy.StopAttack();
    }
}
