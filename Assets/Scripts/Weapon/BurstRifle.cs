using System.Collections;
using UnityEngine;

public class BurstRifle : Weapon
{
    private BurstRifleData _burstRifleData;
    private BurstRifleCharacteristics _burstRifleCharacteristics;

    public override void Init(WeaponData weaponData)
    {
        CurrentWeaponData = weaponData;
        _burstRifleData = weaponData as BurstRifleData;
        WeaponCharacteristics = new BurstRifleCharacteristics(_burstRifleData);
        OnInit();
    }

    public override void Shoot()
    {
        StopAllCoroutines();
        StartCoroutine(Burst());
    }

    private IEnumerator Burst()
    {
        for (int i = 0; i < _burstRifleData.NumberOfBullets; i++)
        {
            GameObject bullet = SpawnBullet();

            Quaternion dir = Quaternion.AngleAxis(0, Vector3.forward);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(dir * transform.up * CurrentWeaponData.BulletSpeed, ForceMode2D.Impulse);
            float scatter = CurrentWeaponData.Scatter;
            float scatterAngle = Random.Range(-scatter, scatter);
            bullet.transform.rotation = Quaternion.Euler(0, 0, scatterAngle + dir.eulerAngles.z + transform.rotation.eulerAngles.z);
            AudioManager.Instance.PlayEffect(CurrentWeaponData.WeaponAudioClip);

            float shootDelay = 0.1f;
            yield return new WaitForSeconds(shootDelay);
        }
    }
}
