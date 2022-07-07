using UnityEngine;

public class StaticEnemyWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponSprite;
    [SerializeField] private Transform _laserSpawnPoint;
    [SerializeField] private SpriteRenderer _laserSpriteRenderer;

    private IEnemyLaserTarget _currentTarget;
    private bool _isAttack;

    public void InitWeapon(StaticEnemyData enemyData)
    {
        _weaponSprite.sprite = enemyData.WeaponData.MainSprite;
        _laserSpawnPoint.localPosition = enemyData.WeaponData.BulletSpawnPosition;
        _laserSpriteRenderer.GetComponent<Animator>().runtimeAnimatorController = enemyData.LaserAnimatorController;
    }

    public void SetTarget(IEnemyLaserTarget target)
    {
        if (_currentTarget != null && _currentTarget.LaserAttackPoint != null)
            _currentTarget.StopLaserInteraction();

        _currentTarget = target;
        _laserSpriteRenderer.enabled = true;
        _currentTarget.StartLaserInteraction();
    }

    public void DestroyLaser()
    {
        if (_currentTarget.LaserAttackPoint != null)
            _currentTarget.StopLaserInteraction();
        _laserSpriteRenderer.enabled = false;
    }

    private void Update()
    {
        ShowLaser();
    }

    private void ShowLaser()
    {
        if (_currentTarget != null)
        {
            if (_currentTarget.LaserAttackPoint != null && _laserSpriteRenderer.enabled)
            {
                _laserSpriteRenderer.size = new Vector2(1, GetLaserLength());
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, GetWeaponAngle()));
            }
            else
            {
                DestroyLaser();
            }
        }
    }

    private float GetLaserLength()
    {
        float laserLength = Vector2.Distance(_laserSpawnPoint.position, _currentTarget.LaserAttackPoint.position);
        return laserLength;
    }

    private float GetWeaponAngle()
    {
        Vector3 targetPosition = _currentTarget.LaserAttackPoint.position;
        Vector3 laserSpawnPosition = _laserSpawnPoint.position;
        float x = targetPosition.x - laserSpawnPosition.x;
        float y = targetPosition.y - laserSpawnPosition.y;
        float angle = -Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        return angle;
    }

    private void OnDestroy()
    {
        if (_currentTarget != null)
            if (_currentTarget.LaserAttackPoint != null)
                _currentTarget.StopLaserInteraction();
    }
}