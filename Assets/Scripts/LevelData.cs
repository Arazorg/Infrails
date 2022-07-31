using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Standart Level", fileName = "New Level")]
public class LevelData : ScriptableObject
{
    [SerializeField] private List<BiomeData> _biomes;
    [SerializeField] private List<AmplificationData> _amplifications;
    [SerializeField] private List<WeaponData> _weapons;
    [SerializeField] private int _enemiesLevel;
    [SerializeField] private int _levelReward;
    [SerializeField] private bool _isBossLevel;

    public List<BiomeData> Biomes => _biomes;

    public List<AmplificationData> Amplifications => _amplifications;

    public List<WeaponData> Weapons => _weapons;

    public int EnemiesLevel => _enemiesLevel;

    public int LevelReward => _levelReward;

    public bool IsBossLevel => _isBossLevel;
}