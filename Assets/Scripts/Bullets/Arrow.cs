using UnityEngine;

public class Arrow : PlayerBullet
{
    [SerializeField] private StickedArrow _stickedArrowPrefab;

    public override void Accept(Transform target)
    {
        StickArrow(target);
        HideBullet();
    }

    public override void Accept(Transform target, IDebuffVisitor hitableVisitor)
    {
        StickArrow(target);
        hitableVisitor.StartBleeding();
        HideBullet();
    }

    private void StickArrow(Transform target)
    {
        if(target.TryGetComponent(out Enemy enemy))
        {
            var stickedArrow = Instantiate(_stickedArrowPrefab, target.position, transform.rotation, target);
            stickedArrow.transform.localPosition = enemy.Data.Center;
            var arrowSprite = GetComponent<SpriteRenderer>().sprite;
            stickedArrow.Init(Data as ArrowData, arrowSprite, target.transform.localScale);
        }
        else if(target.TryGetComponent(out EndWall endWall))
        {
            Instantiate(_stickedArrowPrefab, transform.position, transform.rotation, target);
        }
    }
}
