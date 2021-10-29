public class EnemyAttackState : BaseEnemyState
{
    private IAttackingEnemy _attackingEnemy;

    public EnemyAttackState(IEnemyStateSwitcher stateSwitcher,
        IAttackingEnemy attackingEnemy)
        : base(stateSwitcher)
    {
        _attackingEnemy = attackingEnemy;
    }

    public override void Start()
    {
        _attackingEnemy.AttackTarget();
    }

    public override void Attack()
    {
        _attackingEnemy.AttackTarget();
    }

    public override void Move()
    {
        StateSwitcher.SwitchState<EnemyMovementState>();
    }

    public override void Transform() { }

    public override void Stop() { }
}
