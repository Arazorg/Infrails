using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Standart Character", fileName = "New Character")]

public class CharacterData : UnitData
{
    [SerializeField] private Sprite _characterSprite;
    [SerializeField] private RuntimeAnimatorController _teleportationAnimatorController;
    [SerializeField] private List<Sprite> _hands;
    [SerializeField] private WeaponData _characterStartWeapon;
    [SerializeField] private List<SkillData> _characterSkills;
    [SerializeField] private Vector2 _weaponSpawnPoint;
    [SerializeField] private int _maxArmor;
    [SerializeField] private int _price;

    public Sprite CharacterSprite => _characterSprite;

    public RuntimeAnimatorController TeleportationAnimatorController => _teleportationAnimatorController;

    public List<Sprite> Hands => _hands;

    public WeaponData CharacterStartWeapon => _characterStartWeapon;

    public List<SkillData> CharacterSkills => _characterSkills;

    public Vector2 WeaponSpawnPoint => _weaponSpawnPoint;

    public int MaxArmor => _maxArmor;

    public int Price => _price;
}
