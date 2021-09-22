using System.Collections.Generic;

[System.Serializable]
public class PlayerProgressData
{
    public int PlayerMoney;
    public bool IsLobbyTutorialCompleted;
    public bool IsGameTutorialCompleted;
    public List<CharacterAvailability> CharactersAvailabilities;
    public List<ItemAvailability> WeaponsAvailabilities;
    public List<AmplificationLevel> AmplificationsLevels;
    public List<ItemAvailability> SkillsAvailabilities;

    public PlayerProgressData()
    {
        PlayerMoney = 3250;
        IsLobbyTutorialCompleted = false;
        IsGameTutorialCompleted = false;
        CharactersAvailabilities = new List<CharacterAvailability>();
        WeaponsAvailabilities = new List<ItemAvailability>();
        AmplificationsLevels = new List<AmplificationLevel>();
        SkillsAvailabilities = new List<ItemAvailability>();
    }
}
