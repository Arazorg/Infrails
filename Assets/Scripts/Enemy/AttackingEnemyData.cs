using UnityEngine;

public class AttackingEnemyData : EnemyData
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Vector3 _dizzinesEffectOffset;

    public Coin CoinPrefab => _coinPrefab;

    public WeaponData WeaponData => _weaponData;

    public Vector3 DizzinesOffset => _dizzinesEffectOffset;
}
