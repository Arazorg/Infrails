﻿using System.Collections.Generic;
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
        SetScale();
        Move();
        OnInit();
        BoxCollider2D.enabled = true;
    }

    public void SetEnemyLevel(int enemyLevel)
    {
        if (CurrentGameInfo.Instance.IsInfinite)
            SetHealth(CurrentGameInfo.Instance.ReachedBiomeNumber);
        else
            SetHealth(enemyLevel);
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

    public void StopMove()
    {
        _enemyMovement.OnReachedNextPoint -= SetNextState;
        _enemyAttack.OnAttackFinished -= Move;
        _enemyMovement.StopMove();
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
        _enemyMovement.OnReachedNextPoint -= SetNextState;
        _enemyAttack.StopAttack();
    }

    public void Stun()
    {
        _currentState.Stun();
    }

    public override void BulletHit(PlayerBullet bullet)
    {
        int damageWithResistance = Data.EnemyElement.GetDamageWithResistance(bullet.Damage, bullet.ElementType);
        GetDamage(damageWithResistance);
        bullet.Accept(Transform, this);
    }

    public void StartStunning()
    {
        Stun();
        float stunDuration = 1.25f;
        _enemyDebuffs.StartStunning(this, stunDuration);
    }

    public void StartBleeding()
    {
        _enemyDebuffs.StartBleeding();
    }

    protected override void Death(bool isDeathWithEffect)
    {
        LevelSpawner.Instance.OnBiomeSpawned -= DeathWithoutEffect;
        Character.OnCharacterDeath -= DeathWithoutEffect;
        _enemyAttack.DestroyPoolBullets();

        if (isDeathWithEffect)
        {
            AudioManager.Instance.PlayEffect(Data.DeathAudioClip);
            CurrentGameInfo.Instance.AddEnemyDeath();
            SpawnCoin();
        }

        OnFlyingEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }

    private void InitComponents(Transform spawnPoint)
    {
        _enemy = GetComponent<Enemy>();
        _enemyDebuffs = GetComponent<EnemyDebuffs>();
        _enemyMovement = GetComponent<FlyingEnemyMovement>();
        _enemyAttack = GetComponent<FlyingEnemyAttack>();
        _enemyMovement.Init(spawnPoint.position, Character.CenterPoint);
        _enemyAttack.Init(Character, Data);
    }

    private void InitStates()
    {
        _allStates = new List<BaseEnemyState>()
        {
            new EnemyMovementState(this, this),
            new EnemyAttackState(this, this),
            new EnemyIdleState(this),
            new EnemyStunnedState(this)
        };

        _currentState = _allStates[0];
    }

    private void SetScale()
    {
        float scaleForBiome = 0.0085f;
        float biomeScaleFactor = scaleForBiome * CurrentGameInfo.Instance.ReachedBiomeNumber;
        float minScale = 1.5f + biomeScaleFactor;
        float maxScale = 1.7f + biomeScaleFactor;
        float scaleFactor = Random.Range(minScale, maxScale);
        transform.localScale *= scaleFactor;
    }

    private void SetHealth(int enemyLevel)
    {
        int minHealthForLevel = 1;
        int numberBiomeForGain = 15;
        int bonusHealthForBiomes = 13;
        int healthForLevel = enemyLevel / numberBiomeForGain + minHealthForLevel;
        int bonusHealth = (enemyLevel / numberBiomeForGain) * bonusHealthForBiomes;
        float scaleFactor = transform.localScale.x;
        Health = bonusHealth + (int)((Data.MaxHealth + (healthForLevel * enemyLevel)) * scaleFactor);
    }

    private void SetNextState()
    {
        float attackChance = 0.9f;
        _enemyMovement.OnReachedNextPoint -= SetNextState;

        if (Random.value < attackChance && _enemy.IsGetDamage && CheckDistanceToTarget())
            Attack();
        else
            Move();
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
