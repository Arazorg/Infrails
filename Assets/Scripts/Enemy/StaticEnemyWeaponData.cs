using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Standart Static Enemy Weapon", fileName = "New Static Enemy Weapon")]

public class StaticEnemyWeaponData : ScriptableObject
{
    [SerializeField] private Sprite _mainSprite;
    [SerializeField] private RuntimeAnimatorController _laserAnimatorController;
    [SerializeField] private Vector3 _laserSpawnPosition;

    public Sprite MainSprite => _mainSprite;

    public RuntimeAnimatorController LaserAnimatorController => _laserAnimatorController;

    public Vector3 LaserSpawnPosition => _laserSpawnPosition;
}
