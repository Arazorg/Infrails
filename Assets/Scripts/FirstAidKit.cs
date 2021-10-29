using UnityEngine;

public class FirstAidKit : Enemy
{
    private const int HealValue = 5;

    private Character _character;

    public override void Init(EnemyData data, Transform spawnPoint, GameObject player)
    {
        Data = data;
        OnInit(player);
        TryGetTarget(player);
    }

    protected override void Death()
    {
        _character.Heal(HealValue);
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void TryGetTarget(GameObject target)
    {
        if (target == null)
        {
            EnemiesManager.Instance.OnTargetInit += OnTargetInit;
        }
        else
        {
            _character = target.GetComponent<Character>();
        }
    }

    private void OnTargetInit(GameObject target)
    {
        EnemiesManager.Instance.OnTargetInit -= OnTargetInit;
        _character = target.GetComponent<Character>();
    }
}
