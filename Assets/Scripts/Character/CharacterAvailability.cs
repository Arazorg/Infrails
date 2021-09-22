using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CharacterAvailability : ItemAvailability
{
    public List<ItemAvailability> Skills = new List<ItemAvailability>();

    public CharacterAvailability(string name, bool isAvailable) : base(name, isAvailable)
    {
        Name = name;
        IsAvailable = isAvailable;
    }

    public void AddSkill(string skillName)
    {
        Skills.Add(new ItemAvailability(skillName, true));
    }

    public bool GetSkillAvailability(string skillName)
    {
        return Skills.Where(s => s.Name == skillName).FirstOrDefault() != null;
    }
}
