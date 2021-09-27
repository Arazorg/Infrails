using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private SwitchableButton _musicButton;
    [SerializeField] private SwitchableButton _soundsButton;
    [SerializeField] private SwitchableButton _cameraShakeButton;

    public void Show()
    {
        GetComponent<AnimationsUI>().Show();
        SetButtons();
    }

    public void Hide()
    {
        SetParentUISettingsState();
        GetComponent<AnimationsUI>().Hide();
    }

    public void OnOffMusic()
    {
        SettingsInfo.Instance.IsMusic = !SettingsInfo.Instance.IsMusic;
        _musicButton.SetButtonState(SettingsInfo.Instance.IsMusic);
        SettingsInfo.Instance.Save();
    }

    public void OnOffSounds()
    {
        SettingsInfo.Instance.IsEffects = !SettingsInfo.Instance.IsEffects;
        _soundsButton.SetButtonState(SettingsInfo.Instance.IsEffects);
        SettingsInfo.Instance.Save();
    }

    public void OnOffCameraShake()
    {
        SettingsInfo.Instance.IsCameraShake= !SettingsInfo.Instance.IsCameraShake;
        _cameraShakeButton.SetButtonState(SettingsInfo.Instance.IsCameraShake);
        SettingsInfo.Instance.Save();
    }

    public void ChangeLanguage(string fileName)
    {
        LocalizationManager.LoadLocalizedText(fileName);
    }

    private void SetButtons()
    {
        _musicButton.SetButtonState(SettingsInfo.Instance.IsMusic);
        _soundsButton.SetButtonState(SettingsInfo.Instance.IsEffects);
        _cameraShakeButton.SetButtonState(SettingsInfo.Instance.IsCameraShake);
    }

    private void SetParentUISettingsState()
    {
        if(GetComponentInParent<PauseUI>() != null)
            GetComponentInParent<PauseUI>().IsSettingsPanelOpen = false;
        else if(GetComponentInParent<LobbyUI>() != null)
            GetComponentInParent<LobbyUI>().IsSettingsPanelOpen = false;
    }

}
