using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticEnemy : Enemy, IAttackingEnemy, IMovableEnemy, IEnemyStateSwitcher
{
    private BaseEnemyState _currentState;
    private List<BaseEnemyState> _allStates;
    private StaticEnemyData _staticEnemyData;
    private StaticEnemyMovement _enemyMovement;
    private StaticEnemyAttack _enemyAttack;

    public override void Init(EnemyData data, Transform spawnPoint, GameObject player)
    {
        Data = data;
        _staticEnemyData = Data as StaticEnemyData;
        InitComponents();
        TryGetTarget(player);
        InitStates();
        OnInit(player);
    }

    public void InitScripts(List<Transform> teleportationPoints, List<Enemy> targets)
    {
        _enemyMovement.Init(_staticEnemyData, teleportationPoints);
        _enemyAttack.Init(_staticEnemyData, targets);
    }

    public void SwitchState<T>() where T : BaseEnemyState
    {
        var state = _allStates.FirstOrDefault(s => s is T);
        _currentState.Stop();
        _currentState = state;
        _currentState.Start();
    }

    public void Attack()
    {
        _currentState.Attack();
    }

    public void Move()
    {
        _currentState.Move();
    }

    public void MoveToNextPoint()
    {
        _enemyMovement.Move();
    }

    public void AttackTarget()
    {
        _enemyAttack.OnTargetIsNull += OnTargetIsNull;
        _enemyAttack.Attack();
    }

    protected override void Death()
    {
        Target.GetComponent<Character>().OnCharacterDeath -= Death;
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void InitComponents()
    {
        _enemyAttack = GetComponent<StaticEnemyAttack>();
        _enemyMovement = GetComponent<StaticEnemyMovement>();
    }

    private void TryGetTarget(GameObject target)
    {
        if (target == null)
        {
            EnemiesManager.Instance.OnTargetInit += OnTargetInit;
        }
        else
        {
            Target = target;
            Target.GetComponent<Character>().OnCharacterDeath += Death;
        }
    }

    private void InitStates()
    {
        _allStates = new List<BaseEnemyState>()
        {
            new EnemyIdleState(this, this),
            new EnemyMovementState(this, this),
            new EnemyAttackState(this, this),
            new EnemyTransformationState(this)
        };

        _currentState = _allStates[0];
    }

    private void OnTargetInit(GameObject target)
    {
        EnemiesManager.Instance.OnTargetInit -= OnTargetInit;
        Target = target;
        Target.GetComponent<Character>().OnCharacterDeath += Death;
        Player = target;
    }

    private void OnTargetIsNull()
    {
        if (Player.transform.position.y > transform.position.y)
            Move();
        else
            Attack();
    }

    private void OnBecameVisible()
    {
        Attack();
    }
}
