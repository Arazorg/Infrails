using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Standart Skill", fileName = "New  Skill")]
public class SkillData : ItemData
{
    [SerializeField] private CharacterData _ownerData;
    [SerializeField] private float _skillCooldown;

    public CharacterData OwnerData
    {
        get { return _ownerData; }
    }

    public float SkillCooldown
    {
        get { return _skillCooldown; }
    }
}
