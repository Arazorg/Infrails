using UnityEngine;

public class Rifle : Weapon
{
    public override void Init(WeaponData weaponData)
    {
        CurrentWeaponData = weaponData;
        OnInit();
    }

    public override void Shoot()
    {
        GameObject bullet = SpawnBullet();

        Quaternion dir = Quaternion.AngleAxis(0, Vector3.forward);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(dir * transform.up * CurrentWeaponData.BulletSpeed, ForceMode2D.Impulse);
        bullet.transform.rotation = Quaternion.Euler(0, 0, dir.eulerAngles.z + transform.rotation.eulerAngles.z);
        AudioManager.Instance.PlayEffect(CurrentWeaponData.WeaponAudioClip);
    }
}
