using UnityEngine;

public class FlyingEnemy : Enemy
{
    private EnemyMovement _enemyMovement;

    public override void Init(EnemyData data, Transform spawnPoint, GameObject target)
    {
        Data = data;
        Target = target;
        Target.GetComponent<Character>().OnCharacterDeath += Death;
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyMovement.Init(spawnPoint, target.transform);
        OnInit();
        SetScale();       
    }

    protected override void Death()
    {
        Target.GetComponent<Character>().OnCharacterDeath -= Death;
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void SetScale()
    {
        float minScale = 0.9f;
        float maxScale = 1.35f;
        float scaleFactor = Random.Range(minScale, maxScale);
        transform.localScale *= scaleFactor;
        Health = (int)(Data.MaxHealth * scaleFactor);
    }
}
