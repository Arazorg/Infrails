using UnityEngine;

public class AttackingEnemyData : EnemyData
{
    [SerializeField] private BulletData _bulletData;
    [SerializeField] private int _damage;
    [SerializeField] private float _fireRate;

    public BulletData BulletData
    {
        get { return _bulletData; }
    }

    public int Damage
    {
        get { return _damage; }
    }

    public float FireRate
    {
        get { return _fireRate; }
    }
}
