using TMPro;
using UnityEngine;

public class CharacterSelectionUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private DonationUI _donationUI;
    [SerializeField] private CharacterInfoPanelUI _characterInfoPanelUI;
    [SerializeField] private SkillPanelUI _skillPanelUI;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _characterPriceText;

    [Header("Animations UI Scripts")]
    [SerializeField] private AnimationsUI _confirmButton;
    [SerializeField] private AnimationsUI _buyCharacterButton;

    [SerializeField] private AudioClip _buyCharacterClip;

    private CharacterData _currentCharacterData;
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }

    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    public void OnPush()
    {
        Open();
    }

    public void OnPop()
    {
        Close();
    }

    public void OnShow()
    {
        Open();
    }

    public void OnHide()
    {
        Hide();
    }

    public void SetCharacterData(CharacterData data)
    {
        _currentCharacterData = data;
    }

    public void Open()
    {
        _isBackButtonEnabled = true;
        _isPopAvailable = true;
        CheckCharacterAvailability();
        _characterInfoPanelUI.SetPanelInfo(_currentCharacterData);
        _skillPanelUI.Init(_currentCharacterData);
        _moneyText.text = PlayerProgress.Instance.PlayerMoney.ToString();
        Show();       
    }

    private void Close()
    {
        if (!LobbyEnvironmentManager.Instance.IsCharacterConfirmed)
            LobbyEnvironmentManager.Instance.CancelCharacterSelection();           

        Hide();
    }

    public void GoToLobby()
    {
        UIManager.Instance.UIStackPop();
    } 

    public void ConfirmCharacter()
    {
        LobbyEnvironmentManager.Instance.IsCharacterConfirmed = true;
        CurrentGameInfo.Instance.CharacterData = _currentCharacterData;
        UIManager.Instance.UIStackPop();
    }

    public void BuyCharacter()
    {
        if (PlayerProgress.Instance.PlayerMoney >= _currentCharacterData.Price)
        {
            AudioManager.Instance.PlayEffect(_buyCharacterClip);
            PlayerProgress.Instance.PlayerMoney -= _currentCharacterData.Price;
            PlayerProgress.Instance.SetCharacterAvailable(_currentCharacterData.UnitName);
            CheckCharacterAvailability();
        }
        else
        {
            UIManager.Instance.UIStackPush(_donationUI);
        }
    }
    

    private void CheckCharacterAvailability()
    {
        bool characterIsAvailible = PlayerProgress.Instance.GetCharacterAvailability(_currentCharacterData.UnitName);
        if (characterIsAvailible)
        {
            _buyCharacterButton.Hide();
            _confirmButton.Show();
        }
        else
        {
            _characterPriceText.text = _currentCharacterData.Price.ToString();
            _buyCharacterButton.Show();
        }
    }
}
