using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BossMovement))]
[RequireComponent(typeof(EnemyDebuffs))]
[RequireComponent(typeof(BoxCollider2D))]
public class Boss : Enemy, IEnemyStateSwitcher, IMovableEnemy, IAttackingEnemy, IDebuffVisitor
{
    private BaseEnemyState _currentState;
    private List<BaseEnemyState> _allStates;
    private BossMovement _bossMovement;
    private EnemyDebuffs _enemyDebuffs;
    private Enemy _enemy;

    public delegate void BossDeath();

    public event BossDeath OnBossDeath;

    public override void Init(EnemyData data, Transform spawnPoint, Character character)
    {
        Data = data;
        Character = character;
        InitComponents();
        InitStates();
        Move();
        OnInit();
        BoxCollider2D.enabled = true;
    }

    public override void BulletHit(PlayerBullet bullet)
    {
        int damageWithResistance = Data.EnemyElement.GetDamageWithResistance(bullet.Damage, bullet.ElementType);
        GetDamage(damageWithResistance);
        bullet.Accept(Transform, this);
    }

    public void SwitchState<T>() where T : BaseEnemyState
    {
        var state = _allStates.FirstOrDefault(s => s is T);
        _currentState.Stop();
        _currentState = state;
        _currentState.Start();
    }

    public void StartMove()
    {
        _bossMovement.OnReachedNextPoint += SetNextState;
        _bossMovement.StartMove();
    }
    
    public void Move()
    {
        _currentState.Move();
    }
    
    public void StopMove()
    {
        _bossMovement.OnReachedNextPoint -= SetNextState;
        _bossMovement.StopMove();
    }

    public void StartAttack()
    {
        
    }

    public void StopAttack()
    {
        
    }
    
    public void StartStunning()
    {
        _currentState.Stun();
        float stunDuration = 0.1f;
        _enemyDebuffs.StartStunning(this, stunDuration);
    }

    public void StartBleeding()
    {
        _enemyDebuffs.StartBleeding();
    }
    
    protected override void Death(bool isDeathWithEffect = false)
    {
        OnBossDeath?.Invoke();
    }

    private void InitComponents()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        _enemyDebuffs = GetComponent<EnemyDebuffs>();
        _bossMovement = GetComponent<BossMovement>();
        _bossMovement.Init(Character.CenterPoint);
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
    
    private void SetNextState()
    {
        StopMove();
        Move();
    }
}