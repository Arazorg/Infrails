using UnityEngine;

public class Rifle : Weapon
{
    public override void Init(WeaponData weaponData)
    {
        CurrentWeaponData = weaponData;
        WeaponCharacteristics = new RifleCharacteristics(weaponData);
        OnInit();
    }

    public override void Shoot()
    {
        float scatter = CurrentWeaponData.Scatter;
        float scatterAngle = Random.Range(-scatter, scatter);

        GameObject bullet = SpawnBullet();
        Quaternion dir = Quaternion.AngleAxis(scatterAngle, Vector3.forward);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.AddForce(dir * transform.up * CurrentWeaponData.BulletSpeed, ForceMode2D.Impulse);
        bullet.transform.rotation = Quaternion.Euler(0, 0, dir.eulerAngles.z + transform.rotation.eulerAngles.z);
        AudioManager.Instance.PlayEffect(CurrentWeaponData.WeaponAudioClip);
    }
}
