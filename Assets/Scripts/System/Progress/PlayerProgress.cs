using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance;

    private const string PlayerProgressFile = "PlayerProgress.txt";

    public PlayerProgressData _playerProgressData;
    private SaveSystem _saveSystem;
    private List<AmplificationData> _amplificationsData = new List<AmplificationData>();
    private List<WeaponData> _weaponsData = new List<WeaponData>();
    private List<PassiveSkillData> _passiveSkillsData = new List<PassiveSkillData>();

    public delegate void MoneyUpdate();

    public delegate void SupportDonateComplete();

    public event MoneyUpdate OnMoneyUpdate;

    public event SupportDonateComplete OnSupportDonateComplete;

    public int PlayerMoney
    {
        get 
        { 
            return _playerProgressData.PlayerMoney; 
        }
        set
        {
            _playerProgressData.PlayerMoney = value;
            OnMoneyUpdate?.Invoke();
            Save();
        }
    }

    public bool IsLobbyTutorialCompleted => _playerProgressData.IsLobbyTutorialCompleted;

    public bool IsGameTutorialCompleted => _playerProgressData.IsGameTutorialCompleted;

    public void Load()
    {
        CreateStartPlayerProgressData();
        string currentPlayerProgress = _saveSystem.Load(PlayerProgressFile);
        if (currentPlayerProgress != string.Empty)
            WriteCurrentProgress(currentPlayerProgress);

        Save();
    }

    public void Save()
    {
        string currentPlayerProgress = JsonUtility.ToJson(_playerProgressData);
        _saveSystem.Save(currentPlayerProgress, PlayerProgressFile);
    }

    public void SetLobbyTutorialComplete()
    {
        _playerProgressData.IsLobbyTutorialCompleted = true;
        Save();
    }

    public void SetGameTutorialComplete()
    {
        _playerProgressData.IsGameTutorialCompleted = true;
        Save();
    }

    public bool GetTrolleyForSupportAvailability()
    {
        return _playerProgressData.TrolleyForSupportAvailability.IsAvailable;
    }

    public void SetTrolleyForSupportAvailable()
    {
        _playerProgressData.TrolleyForSupportAvailability.IsAvailable = true;
        PlayerPrefs.SetInt(GameConstants.TrolleyForSupportKey, 1);
        PlayerPrefs.Save();
        Save();
    }

    public void SupportDonate()
    {
        OnSupportDonateComplete?.Invoke();
    }


    #region Characters
    public bool GetCharacterAvailability(string characterName)
    {
        return GetCharacterByName(characterName).IsAvailable;
    }

    public void SetCharacterAvailable(string characterName)
    {
        GetCharacterByName(characterName).IsAvailable = true;
        Save();
    }
    #endregion Characters

    #region Weapons
    public List<WeaponData> GetWeaponsData(bool isAvailable)
    {
        List<WeaponData> weapons = new List<WeaponData>();
        foreach (var weapon in _weaponsData)
        {
            if (isAvailable)
            {
                if (GetWeaponByName(weapon.ItemName).IsAvailable)
                {
                    weapons.Add(weapon);
                }
            }
            else
            {
                if (!GetWeaponByName(weapon.ItemName).IsAvailable)
                {
                    weapons.Add(weapon);
                }
            }
        }

        return weapons;
    }

    public void SetWeaponAvailable(string weaponName)
    {
        GetWeaponByName(weaponName).IsAvailable = true;
        Save();
    }
    #endregion Weapons

    #region Amplifications
    public List<AmplificationData> GetAmplificationsData(bool isAvailable)
    {
        List<AmplificationData> amplificationsData = new List<AmplificationData>();
        foreach (var amplification in _playerProgressData.AmplificationsLevels)
        {
            var data = GetAmplificationDataByName(amplification.Name);
            data.Level = GetAmplificationLevelByName(amplification.Name).Level;
            if (isAvailable)
            {
                if (data.Level != 0)
                {
                    amplificationsData.Add(data);
                }
            }
            else
            {
                if (data.Level != 3)
                {
                    amplificationsData.Add(data);
                }
            }

        }
        return amplificationsData;
    }

    public void SetAmplificationLevel(string amplificationName, int level)
    {
        GetAmplificationLevelByName(amplificationName).Level = level;
    }

    public void IncrementAmplificationLevel(string amplificationName)
    {
        GetAmplificationLevelByName(amplificationName).IncrementLevel();
        Save();
    }
    #endregion Amplifications

    #region PassiveSkills
    public bool GetPassiveSkillAvailability(string characterName, string skillName)
    {
        return GetCharacterByName(characterName).GetSkillAvailability(skillName);
    }

    public void SetPassiveSkillAvailable(string characterName, string skillName)
    {
        GetPassiveSkillByName(skillName).IsAvailable = true;
        GetCharacterByName(characterName).AddSkill(skillName);
        Save();
    }
    #endregion Skills

    public List<ItemData> GetLootsAvailableInShop(LootboxData lootboxData)
    {
        switch (lootboxData.TypeOfLootbox)
        {
            case LootboxData.Type.Amplification:
                return GetAmplificationsData(GameConstants.Unavailable).Cast<ItemData>().ToList();
            case LootboxData.Type.Weapon:
                return GetWeaponsData(GameConstants.Unavailable).Cast<ItemData>().ToList();
            case LootboxData.Type.Skill:
                return GetUnavailablePassiveSkillsData().Cast<ItemData>().ToList();
            default:
                return new List<ItemData>();
        }
    }

    public bool CheckLootsAvailability(LootboxData lootboxData)
    {
        return GetLootsAvailableInShop(lootboxData).Count != 0;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _saveSystem = new SaveSystem();
    }

    private void CreateStartPlayerProgressData()
    {
        string startAvailableCharacterName = "Jonh";
        _playerProgressData = new PlayerProgressData();
        LoadCharacters();
        LoadAmplifications();
        LoadWeapons();
        LoadPassiveSkills();
        GetCharacterByName(startAvailableCharacterName).IsAvailable = true;
    }

    private void LoadCharacters()
    {
        string charactersDataPath = "Specifications/Characters";
        var charactersData = Resources.LoadAll<CharacterData>(charactersDataPath);

        foreach (var character in charactersData)
            _playerProgressData.CharactersAvailabilities.Add(new CharacterAvailability(character.UnitName, false));
    }

    private void LoadWeapons()
    {
        string weaponsDataPath = "Specifications/Weapons";
        var weaponsData = Resources.LoadAll<WeaponData>(weaponsDataPath).ToList();
        _weaponsData = weaponsData;
        foreach (var weapon in weaponsData)
        {
            var weaponAvailability = new ItemAvailability(weapon.ItemName, false);
            if (weapon.IsStartingItem)
                weaponAvailability.IsAvailable = true;

            _playerProgressData.WeaponsAvailabilities.Add(weaponAvailability);
        }
    }

    private void LoadAmplifications()
    {
        string amplificationsDataPath = "Specifications/Amplifications";
        var amplificationsData = Resources.LoadAll<AmplificationData>(amplificationsDataPath).ToList();
        _amplificationsData = amplificationsData;

        foreach (var amplification in amplificationsData)
        {
            var amplificationAvailability = new AmplificationLevel(amplification.ItemName);
            if (amplification.IsStartingItem)
                amplificationAvailability.IncrementLevel();

            _playerProgressData.AmplificationsLevels.Add(amplificationAvailability);
        }
    }

    private void LoadPassiveSkills()
    {
        string passiveSkillsDataPath = "Specifications/PassiveSkills";
        
        var passiveSkillsData = Resources.LoadAll<PassiveSkillData>(passiveSkillsDataPath).ToList();
        _passiveSkillsData = passiveSkillsData;
        foreach (var skill in passiveSkillsData)
            _playerProgressData.PassiveSkillsAvailabilities.Add(new ItemAvailability(skill.ItemName, false));
    }

    private void WriteCurrentProgress(string currentPlayerProgress)
    {
        var tempData = JsonUtility.FromJson<PlayerProgressData>(currentPlayerProgress);
        _playerProgressData.PlayerMoney = tempData.PlayerMoney;
        _playerProgressData.IsLobbyTutorialCompleted = tempData.IsLobbyTutorialCompleted;
        _playerProgressData.IsGameTutorialCompleted = tempData.IsGameTutorialCompleted;
        _playerProgressData.TrolleyForSupportAvailability.IsAvailable = tempData.TrolleyForSupportAvailability.IsAvailable;
        if (_playerProgressData.TrolleyForSupportAvailability.IsAvailable)
            PlayerPrefs.SetInt(GameConstants.TrolleyForSupportKey, 1);

        foreach (var character in tempData.CharactersAvailabilities)
        {
            if (GetCharacterByName(character.Name) != null && character.IsAvailable)
                SetCharacterAvailable(character.Name);

            foreach (var skill in character.PassiveSkills)
            {
                if (skill.IsAvailable)
                    SetPassiveSkillAvailable(character.Name, skill.Name);
            }
        }

        foreach (var amplification in tempData.AmplificationsLevels)
        {
            if (GetAmplificationLevelByName(amplification.Name) != null && amplification.Level != 0)
                SetAmplificationLevel(amplification.Name, amplification.Level);
        }

        foreach (var weapon in tempData.WeaponsAvailabilities)
        {
            if (GetWeaponByName(weapon.Name) != null && weapon.IsAvailable)
                SetWeaponAvailable(weapon.Name);
        }

        foreach (var skill in tempData.PassiveSkillsAvailabilities)
        {
            if (GetWeaponByName(skill.Name) != null && skill.IsAvailable)
                GetPassiveSkillByName(skill.Name).IsAvailable = true;
        }
    }

    private List<PassiveSkillData> GetUnavailablePassiveSkillsData()
    {
        List<PassiveSkillData> skills = new List<PassiveSkillData>();
        foreach (var skill in _passiveSkillsData)
        {
            if (!GetPassiveSkillByName(skill.ItemName).IsAvailable)
                skills.Add(skill);
        }

        return skills;
    }

    private CharacterAvailability GetCharacterByName(string characterName)
    {
        return _playerProgressData.CharactersAvailabilities.Where(s => s.Name == characterName).FirstOrDefault();
    }

    private ItemAvailability GetWeaponByName(string weaponName)
    {
        return _playerProgressData.WeaponsAvailabilities.Where(s => s.Name == weaponName).FirstOrDefault();
    }

    private ItemAvailability GetPassiveSkillByName(string skillName)
    {
        return _playerProgressData.PassiveSkillsAvailabilities.Where(s => s.Name == skillName).FirstOrDefault();
    }
    private AmplificationLevel GetAmplificationLevelByName(string amplificationName)
    {
        return _playerProgressData.AmplificationsLevels.Where(s => s.Name == amplificationName).FirstOrDefault();
    }

    private AmplificationData GetAmplificationDataByName(string amplificationName)
    {
        return _amplificationsData.Where(s => s.ItemName == amplificationName).FirstOrDefault();
    }
}
