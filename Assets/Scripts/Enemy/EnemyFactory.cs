using UnityEngine;

public class EnemyFactory : GenericFactory<Enemy>
{
    public Enemy GetEnemy(Enemy enemyPrefab, Transform spawnPoint)
    {
        return GetNewInstanceToParent(enemyPrefab, spawnPoint);
    }

    public Enemy GetEnemyByPosition(Enemy enemyPrefab, Vector3 position)
    {
        return GetNewInstanceByPosition(enemyPrefab, position);
    }
}
