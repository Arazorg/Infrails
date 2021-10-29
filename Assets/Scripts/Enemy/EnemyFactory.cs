using UnityEngine;

public class EnemyFactory : GenericFactory<Enemy>
{
    public Enemy GetEnemy(Enemy enemyPrefab, Transform spawnPoint)
    {
        return GetNewInstanceToParent(enemyPrefab, spawnPoint);
    }
}
