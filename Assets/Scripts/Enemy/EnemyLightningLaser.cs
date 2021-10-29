using System.Collections;
using UnityEngine;

public class EnemyLightningLaser : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _weaponSprite;
    [SerializeField] private Transform _laserSpawnPoint;
    [SerializeField] private SpriteRenderer _laserSpriteRenderer;

    private Enemy _currentTarget;
    private StaticEnemyWeaponData _weaponData;

    public void InitWeapon(StaticEnemyWeaponData weaponData)
    {
        _weaponData = weaponData;
        _weaponSprite.sprite = weaponData.MainSprite;
        _laserSpawnPoint.localPosition = weaponData.LaserSpawnPosition;
        _laserSpriteRenderer.GetComponent<Animator>().runtimeAnimatorController = weaponData.LaserAnimatorController;
    }

    public void SetTarget(Enemy target)
    {
        _currentTarget = target;
        StartCoroutine(DamageTarget());
    }

    private void Update()
    {
        SpawnLaser();
    }

    private void SpawnLaser()
    {
        if (_currentTarget != null)
        {
            _laserSpriteRenderer.enabled = true;
            float laserLength = Vector2.Distance(_laserSpawnPoint.position, _currentTarget.LighthingLaserPoint.position);
            _laserSpriteRenderer.size = new Vector2(1, laserLength);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, GetWeaponAngle()));
        }
        else
        {
            StopAllCoroutines();
            _laserSpriteRenderer.enabled = false;
        }
    }

    private float GetWeaponAngle()
    {
        Vector3 targetPosition = _currentTarget.LighthingLaserPoint.position;
        Vector3 laserSpawnPosition = _laserSpawnPoint.position;
        float x = targetPosition.x - laserSpawnPosition.x;
        float y = targetPosition.y - laserSpawnPosition.y;
        float angle = -Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        return angle;
    }

    private IEnumerator DamageTarget()
    {
        yield return new WaitForSeconds(2f);
        _currentTarget.DestroyWithoutDeathEffect();
    }
}
