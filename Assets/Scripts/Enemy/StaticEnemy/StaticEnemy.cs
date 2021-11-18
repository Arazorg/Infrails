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
    private Transform _characterTransform;
    private bool _isEndOfBiomeReached;

    public override void Init(EnemyData data, Transform spawnPoint, Character character)
    {
        Data = data;
        _staticEnemyData = Data as StaticEnemyData;
        InitComponents();
        TryGetCharacter(character);
        InitStates();
        OnInit();
    }

    public void InitScripts(List<Transform> teleportationPoints, List<IEnemyLaserTarget> targets)
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

    public void Idle()
    {
        _currentState.Idle();
    }

    public void Move()
    {
        _currentState.Move();
    }

    public void StartMove()
    {
        _enemyMovement.OnReachedNextPoint += Attack;
        if (!_enemyMovement.Move())
        {
            _isEndOfBiomeReached = true;
            Attack();
        }
    }

    public void Attack()
    {
        _enemyMovement.OnReachedNextPoint -= Attack;
        _currentState.Attack();
    }

    public void StartAttack()
    {
        _enemyAttack.OnTargetBecameNull += OnTargetBecameNull;
        _enemyAttack.StartAttack();
    }

    public void StopAttack()
    {
        _enemyAttack.OnTargetBecameNull -= OnTargetBecameNull;
        _enemyAttack.StopAttack();
    }

    protected override void Death(bool isDeathWithEffect)
    {
        Character.OnCharacterDeath -= DeathWithoutEffect;
        if (isDeathWithEffect)
            SpawnCoin();

        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void InitComponents()
    {
        _enemyAttack = GetComponent<StaticEnemyAttack>();
        _enemyMovement = GetComponent<StaticEnemyMovement>();
    }

    private void TryGetCharacter(Character character)
    {
        if (character == null)
        {
            EnemiesManager.Instance.OnCharacterAvailable += OnCharacterAvailable;
        }
        else
        {
            Character = character;
            _characterTransform = Character.Transform;
            _enemyAttack.Character = Character;
            Character.OnCharacterDeath += DeathWithoutEffect;
        }
    }

    private void InitStates()
    {
        _allStates = new List<BaseEnemyState>()
        {
            new EnemyIdleState(this),
            new EnemyMovementState(this, this),
            new EnemyAttackState(this, this),
        };

        _currentState = _allStates[0];
    }

    private void Update()
    {
        CheckDistanceToCharacter();
    }

    private void CheckDistanceToCharacter()
    {
        float distanceToMoveY = 12.5f;
        if (_characterTransform != null)
        {
            float distanceY = transform.position.y - _characterTransform.position.y;
            if (distanceY < distanceToMoveY && !_isEndOfBiomeReached)
                Move();
        }
    }

    private void OnCharacterAvailable(Character character)
    {
        Character = character;
        _characterTransform = Character.Transform;
        _enemyAttack.Character = Character;
        Character.OnCharacterDeath += DeathWithoutEffect;
    }

    private void SpawnCoin()
    {
        var coinPrefab = (Data as AttackingEnemyData).CoinPrefab;
        Coin coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        coin.Init(Character.Transform);
    }

    private void OnTargetBecameNull()
    {
        _enemyAttack.OnTargetBecameNull -= OnTargetBecameNull;
        Attack();
    }

    private void DeathWithoutEffect()
    {
        Death(GameConstants.DeathWithoutEffect);
    }

    private void OnBecameVisible()
    {
        IsGetDamage = true;
    }

    private void OnBecameInvisible()
    {
        IsGetDamage = false;
        _enemyAttack.OnTargetBecameNull -= OnTargetBecameNull;
        Idle();
    }

    private void OnDestroy()
    {
        if (Character != null)
            Character.OnCharacterDeath -= DeathWithoutEffect;
    }
}
