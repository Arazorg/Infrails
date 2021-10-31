using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyingEnemy : Enemy, IAttackingEnemy, IMovableEnemy, IEnemyStateSwitcher
{
    private BaseEnemyState _currentState;
    private List<BaseEnemyState> _allStates;

    public Transform NextPoint { get ; set; }

    public override void Init(EnemyData data, Transform spawnPoint, GameObject target)
    {
        Data = data;
        Target = target;
        Target.GetComponent<Character>().OnCharacterDeath += Death;
        InitStates();
        OnInit();
        SetScale();       
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
        _currentState.Move();
    }

    public void Attack()
    {
        _currentState.Attack();
    }

    public Transform GetNextPoint()
    {
        return transform;
    }

    public void GetTarget()
    {
        
    }

    protected override void Death()
    {
        Target.GetComponent<Character>().OnCharacterDeath -= Death;
        SpawnExplosionParticle();
        Destroy(gameObject);
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

    public void AttackTarget()
    {
        throw new System.NotImplementedException();
    }

    public void MoveToNextPoint()
    {
        throw new System.NotImplementedException();
    }

    public void StopAttack()
    {
        throw new System.NotImplementedException();
    }

    public void StartAttack()
    {
        throw new System.NotImplementedException();
    }
}
