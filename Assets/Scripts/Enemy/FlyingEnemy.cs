using UnityEngine;

public class FlyingEnemy : Enemy
{
    public override void Death()
    {
        
    }

    private void SetScaleFactorParams()
    {
        float minScale = 0.9f;
        float maxScale = 1.35f;
        float scaleFactor = Random.Range(minScale, maxScale);

        transform.localScale *= scaleFactor;
        boxCollider2D.size = data.ColliderSize * scaleFactor;
        boxCollider2D.offset = data.ColliderOffset;
        _health = (int)(data.MaxHealth * scaleFactor);
    }

    private void InitMovementScript(Transform characterTransform, Transform spawnPoint)
    {
        var movementScript = gameObject.AddComponent<EnemyMovement>();
        movementScript.Target = characterTransform;
        movementScript.SpawnPoint = spawnPoint;
        movementScript.IsChase = true;
    }
}
