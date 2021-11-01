using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyingEnemy : Enemy, IAttackingEnemy, IMovableEnemy, IEnemyStateSwitcher
{
    private BaseEnemyState _currentState;
    private List<BaseEnemyState> _allStates;
    private FlyingEnemyMovement _enemyMovement;
    private FlyingEnemyAttack _enemyAttack;
    private Enemy _enemy;

    public delegate void EnemyDeath(FlyingEnemy enemy);

    public event EnemyDeath OnEnemyDeath;

    public Transform NextPoint { get; set; }

    public override void Init(EnemyData data, Transform spawnPoint, GameObject target)
    {
        Data = data;
        Target = target;
        Target.GetComponent<Character>().OnCharacterDeath += Death;
        LevelSpawner.Instance.OnBiomeSpawned += Death;
        InitComponents(spawnPoint, target);
        InitStates();
        OnInit();
        SetScale();       
        Move();
    }

    public void SwitchState<T>() where T : BaseEnemyState
    {
        var state = _allStates.FirstOrDefault(s => s is T);
        _currentState.Stop();
        _currentState = state;
        _currentState.Start();
    }

    public void Move()
    {
        _enemyAttack.OnAttackFinished -= Move;
        _currentState.Move();
    }

    public void StartMove()
    {
        _enemyMovement.OnReachedNextPoint += SetNextState;
        _enemyMovement.StartMove();
    }

    public void Attack()
    {
        _enemyMovement.OnReachedNextPoint -= SetNextState;
        _currentState.Attack();
    }

    public void StartAttack()
    {
        _enemyAttack.OnAttackFinished += Move;
        _enemyAttack.StartAttack();
    }

    public void StopAttack()
    {
        _enemyAttack.OnAttackFinished -= Move;
        _enemyAttack.StopAttack();
    }

    protected override void Death()
    {
        LevelSpawner.Instance.OnBiomeSpawned -= Death;
        OnEnemyDeath?.Invoke(this);
        Target.GetComponent<Character>().OnCharacterDeath -= Death;
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void InitComponents(Transform spawnPoint, GameObject target)
    {
        _enemy = GetComponent<Enemy>();
        _enemyMovement = GetComponent<FlyingEnemyMovement>();
        _enemyAttack = GetComponent<FlyingEnemyAttack>();
        _enemyMovement.Init(spawnPoint.position, target.transform);
        _enemyAttack.Init(target.transform, Data);
    }

    private void InitStates()
    {
        _allStates = new List<BaseEnemyState>()
        {
            new EnemyMovementState(this, this),
            new EnemyAttackState(this, this),
        };

        _currentState = _allStates[0];
    }

    private void SetScale()
    {
        float minScale = 1f;
        float maxScale = 1.5f;
        float scaleFactor = Random.Range(minScale, maxScale);
        transform.localScale *= scaleFactor;
        Health = (int)(Data.MaxHealth * scaleFactor);
    }

    private void SetNextState()
    {
        float attackChance = 0.4f;
        _enemyMovement.OnReachedNextPoint -= SetNextState;
 
        if (Random.value < attackChance && _enemy.IsGetDamage && CheckDistanceToTarget())
            StartAttack();
        else
            StartMove();
    }

    private bool CheckDistanceToTarget()
    {
        float distanceY = transform.position.y - Target.transform.position.y;
        float minDistanceToAttack = 17.5f;
        float maxDistanceToAttack = 35f;

        return distanceY > minDistanceToAttack && distanceY < maxDistanceToAttack;
    }
}
