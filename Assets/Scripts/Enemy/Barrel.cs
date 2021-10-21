using UnityEngine;

public class Barrel : Enemy
{
    public override void Init(EnemyData data, Transform spawnPoint, GameObject target)
    {
        Data = data;
        OnInit();
    }

    protected override void Death()
    {
        SpawnExplosionParticle();
        Destroy(gameObject);
    }
}
