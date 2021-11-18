using UnityEngine;

public class Arrow : Bullet
{
    [SerializeField] private StickedArrow _stickedArrowPrefab;

    public override void BulletHit(Transform target)
    {
        StickArrow(target);
        HideBullet();
    }

    private void StickArrow(Transform target)
    {
        if(target.TryGetComponent(out Enemy enemy))
        {
            Vector3 spawnPosition = enemy.ArrowSpawnPoint.position;
            enemy.ArrowHitEffect();
            var stickedArrow = Instantiate(_stickedArrowPrefab, spawnPosition, transform.rotation, target);
            var arrowSprite = GetComponent<SpriteRenderer>().sprite;
            stickedArrow.Init(Data as ArrowData, arrowSprite, target.transform.localScale);
        }
    }
}
