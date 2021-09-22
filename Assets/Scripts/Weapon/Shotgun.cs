using UnityEngine;

public class Shotgun : Weapon
{
    public override void Shoot()
    {
        int countOfBullets = Random.Range(5, 7);
        for (int i = 0; i < countOfBullets; i++)
        {
            var bullet = BulletSpawner.SpawnBullet(CurrentWeaponData, CurrentElement);
            Quaternion dir = Quaternion.AngleAxis(Random.Range(-25f, 25f), Vector3.forward);
            float speed = CurrentWeaponData.BulletSpeed * Random.Range(0.8f, 1.2f);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(dir * transform.up * speed, ForceMode2D.Impulse);
        }
    }
}
