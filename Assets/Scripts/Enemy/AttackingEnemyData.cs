using UnityEngine;

public class AttackingEnemyData : EnemyData
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private EnemyWeaponData _weaponData;
    [SerializeField] private Vector3 _dizzinesEffectOffset;

    public Coin CoinPrefab => _coinPrefab;

    public EnemyWeaponData WeaponData => _weaponData;

    public Vector3 DizzinesOffset => _dizzinesEffectOffset;
}
