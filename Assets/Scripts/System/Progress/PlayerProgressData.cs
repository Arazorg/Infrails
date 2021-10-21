using System.Collections.Generic;

[System.Serializable]
public class PlayerProgressData
{
    private const string TrolleyForSupportName = "TrolleyForSupport";

    public int PlayerMoney;
    public bool IsLobbyTutorialCompleted;
    public bool IsGameTutorialCompleted;
    public ItemAvailability TrolleyForSupportAvailability;
    public List<ItemAvailability> SkillsAvailabilities;
    public List<ItemAvailability> WeaponsAvailabilities;
    public List<AmplificationLevel> AmplificationsLevels;
    public List<CharacterAvailability> CharactersAvailabilities;

    public PlayerProgressData()
    {
        PlayerMoney = 50000;
        IsLobbyTutorialCompleted = false;
        IsGameTutorialCompleted = false;
        TrolleyForSupportAvailability = new ItemAvailability(TrolleyForSupportName, false);
        SkillsAvailabilities = new List<ItemAvailability>();
        WeaponsAvailabilities = new List<ItemAvailability>();
        AmplificationsLevels = new List<AmplificationLevel>();
        CharactersAvailabilities = new List<CharacterAvailability>();
    }
}
