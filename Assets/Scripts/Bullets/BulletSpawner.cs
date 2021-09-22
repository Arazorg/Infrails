using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;

    public Vector3 BulletSpawnPosition
    {
        set { bulletSpawnPoint.localPosition = value; }
    }

    public GameObject SpawnBullet(BulletData bulletData, int damage, float critChance, ElementsResistance.Elements currentElement)
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Init(bulletData, damage, critChance, currentElement);
        return bullet.gameObject;
    }

    public GameObject SpawnBullet(WeaponData weaponData, ElementsResistance.Elements currentElement)
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Init(weaponData.BulletData, weaponData.Damage, 0, currentElement);
        return bullet.gameObject;
    }
}
