[System.Serializable]
public class SettingsData
{
    public string CurrentLocalization;
    public bool IsMusic;
    public bool IsEffects;
    public bool IsCameraShake;

    public SettingsData()
    {
        CurrentLocalization = "localizedText_ru";
        IsMusic = true;
        IsEffects = true;
        IsCameraShake = true;
    }
}