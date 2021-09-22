using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CharacterProgressData
{
    public string CharacterName;
    public bool IsAvailable;
   

    public CharacterProgressData(string characterName, bool isAvailable)
    {
        CharacterName = characterName;
        IsAvailable = isAvailable;
    }

}
