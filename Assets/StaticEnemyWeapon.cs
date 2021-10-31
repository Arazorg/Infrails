using UnityEngine;

public class StaticEnemyWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponSprite;
    [SerializeField] private Transform _laserSpawnPoint;
    [SerializeField] private SpriteRenderer _laserSpriteRenderer;

    private IEnemyLaserTarget _currentTarget;

    public void InitWeapon(StaticEnemyWeaponData weaponData)
    {
        _weaponSprite.sprite = weaponData.MainSprite;
        _laserSpawnPoint.localPosition = weaponData.LaserSpawnPosition;
        _laserSpriteRenderer.GetComponent<Animator>().runtimeAnimatorController = weaponData.LaserAnimatorController;
    }

    public void SetTarget(IEnemyLaserTarget target)
    {
        if(_currentTarget != null && _currentTarget.LaserAttackPoint != null)
            _currentTarget.StopLaserInteraction();

        _currentTarget = target;
        _laserSpriteRenderer.enabled = true;
        _currentTarget.StartLaserInteraction();
    }

    public void DestroyLaser()
    {
        _laserSpriteRenderer.enabled = false;
    }

    private void Update()
    {
        ShowLaser();
    }

    private void ShowLaser()
    {
        if(_currentTarget != null)
        {
            if (_currentTarget.LaserAttackPoint != null)
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
            _currentTarget.StopLaserInteraction();
    }
}
