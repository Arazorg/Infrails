using UnityEngine;

public class AttackingEnemyData : EnemyData
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private WeaponData _weaponData;

    public Coin CoinPrefab => _coinPrefab;

    public WeaponData WeaponData => _weaponData;
}
