using UnityEngine;

public class SettingsInfo : MonoBehaviour
{
    public static SettingsInfo Instance;

    private const string SettingsFile = "Settings.txt";

    public SettingsData _settingsData;
    private SaveSystem _saveSystem;

    public string CurrentLocalization
    {
        get { return _settingsData.CurrentLocalization; }
        set { _settingsData.CurrentLocalization = value; }
    }

    public bool IsMusic
    {
        get
        {
            return _settingsData.IsMusic;
        }

        set
        {
            _settingsData.IsMusic = value;
            if (IsMusic)
            {
                AudioManager.Instance.PlayMusic();
            }
            else
            {
                AudioManager.Instance.StopMusic();
            }
        }
    }

    public bool IsEffects
    {
        get
        {
            return _settingsData.IsEffects;
        }

        set
        {
            _settingsData.IsEffects = value;
            if (!IsEffects)
            {
                AudioManager.Instance.StopAllEffects();
            }
        }
    }

    public bool IsCameraShake
    {
        get { return _settingsData.IsCameraShake; }
        set { _settingsData.IsCameraShake = value; }
    }

    public void Load()
    {
        CreateStartSettingsData();
        string currentSettings = _saveSystem.Load(SettingsFile);
        if (currentSettings != string.Empty)
        {
            _settingsData = JsonUtility.FromJson<SettingsData>(currentSettings);
        }           
        Save();
    }

    public void Save()
    {
        string currentSettings = JsonUtility.ToJson(_settingsData);
        _saveSystem.Save(currentSettings, SettingsFile);
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

    private void CreateStartSettingsData()
    {
        _settingsData = new SettingsData();
    }
}
