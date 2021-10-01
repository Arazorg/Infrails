using UnityEngine;

public class Shotgun : Weapon
{
    private ShotgunData _shotgunData;

    public override void Init(WeaponData weaponData)
    {
        CurrentWeaponData = weaponData;
        _shotgunData = weaponData as ShotgunData;
        OnInit();
    }

    public override void Shoot()
    {
        int numberOfBullets = Random.Range(_shotgunData.MinNumberOfBullets, _shotgunData.MaxNumberOfBullets);

        for (int i = 0; i < numberOfBullets; i++)
        {
            var bullet = SpawnBullet();

            float angle = Random.Range(-_shotgunData.ScatterAngle, _shotgunData.ScatterAngle);
            float speedFactor = Random.Range((1 - _shotgunData.BulletSpeedSpread), (1 + _shotgunData.BulletSpeedSpread));
            float speed = CurrentWeaponData.BulletSpeed * speedFactor;

            Quaternion dir = Quaternion.AngleAxis(angle, Vector3.forward);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(dir * transform.up * speed, ForceMode2D.Impulse);
            bullet.transform.rotation = Quaternion.Euler(0, 0, dir.eulerAngles.z + transform.rotation.eulerAngles.z);
        }
    }
}
