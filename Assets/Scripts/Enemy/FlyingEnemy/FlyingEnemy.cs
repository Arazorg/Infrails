﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyingEnemy : Enemy, IAttackingEnemy, IMovableEnemy, IEnemyStateSwitcher, IDebuffVisitor
{
    private BaseEnemyState _currentState;
    private List<BaseEnemyState> _allStates;
    private FlyingEnemyMovement _enemyMovement;
    private FlyingEnemyAttack _enemyAttack;
    private EnemyDebuffs _enemyDebuffs;
    private Enemy _enemy;

    public delegate void FlyingEnemyDeath(FlyingEnemy enemy);

    public event FlyingEnemyDeath OnFlyingEnemyDeath;

    public Transform NextPoint { get; set; }

    public override void Init(EnemyData data, Transform spawnPoint, Character character)
    {
        Data = data;
        Character = character;
        Character.OnCharacterDeath += DeathWithoutEffect;
        LevelSpawner.Instance.OnBiomeSpawned += DeathWithoutEffect;
        InitComponents(spawnPoint);
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

    public void Idle()
    {
        _currentState.Idle();
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

    public void StartStunning()
    {
        _enemyDebuffs.StartStunning();
    }

    public void StartBleeding()
    {
        _enemyDebuffs.StartBleeding();
    }

    public override void BulletHit(Bullet bullet)
    {
        GetDamage(bullet.Damage);
        bullet.Accept(Transform, this);
    }

    protected override void Death(bool isDeathWithEffect)
    {
        LevelSpawner.Instance.OnBiomeSpawned -= DeathWithoutEffect;
        Character.OnCharacterDeath -= DeathWithoutEffect;
        _enemyAttack.DestroyPoolBullets();

        if (isDeathWithEffect)
            SpawnCoin();

        OnFlyingEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }

    private void InitComponents(Transform spawnPoint)
    {
        _enemy = GetComponent<Enemy>();
        _enemyDebuffs = GetComponent<EnemyDebuffs>();
        _enemyMovement = GetComponent<FlyingEnemyMovement>();
        _enemyAttack = GetComponent<FlyingEnemyAttack>();
        _enemyMovement.Init(spawnPoint.position, Character.Transform);
        _enemyAttack.Init(Character.Transform, Data);
    }

    private void InitStates()
    {
        _allStates = new List<BaseEnemyState>()
        {
            new EnemyMovementState(this, this),
            new EnemyAttackState(this, this),
            new EnemyIdleState(this)
        };

        _currentState = _allStates[0];
    }

    private void SetScale()
    {
        float minScale = 1.2f;
        float maxScale = 1.75f;
        float scaleFactor = Random.Range(minScale, maxScale);
        transform.localScale *= scaleFactor;
        Health = (int)(Data.MaxHealth * scaleFactor);
    }

    private void SetNextState()
    {
        float attackChance = 0.75f;
        _enemyMovement.OnReachedNextPoint -= SetNextState;

        if (Random.value < attackChance && _enemy.IsGetDamage && CheckDistanceToTarget())
            StartAttack();
        else
            StartMove();
    }

    private bool CheckDistanceToTarget()
    {
        float distanceY = transform.position.y - Character.Transform.position.y;
        float minDistanceToAttack = 17.5f;
        float maxDistanceToAttack = 35f;

        return distanceY > minDistanceToAttack && distanceY < maxDistanceToAttack;
    }

    private void SpawnCoin()
    {
        var coinPrefab = (Data as AttackingEnemyData).CoinPrefab;
        Coin coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        coin.Init(Character.Transform);
    }

    private void DeathWithoutEffect()
    {
        Death(GameConstants.DeathWithoutEffect);
    }
}
