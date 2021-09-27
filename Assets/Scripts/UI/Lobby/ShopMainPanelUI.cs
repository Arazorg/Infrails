using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

public class ShopMainPanelUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private DonationUI _donationUI;
    [SerializeField] private ShopLootPanelUI _lootPanelUI;
    [SerializeField] private LootboxInfoPanelUI _lootboxInfoPanelUI;

    [Header("Animations UI Scripts")]
    [SerializeField] private AnimationsUI _openLootboxButton;
    [SerializeField] private AnimationsUI _showAdButton;
    [SerializeField] private AnimationsUI _lootboxInfoText;
    [SerializeField] private AnimationsUI _prevButton;
    [SerializeField] private AnimationsUI _nextButton;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _moneyClip;
    [SerializeField] private AudioClip _clickClip;

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private List<LootboxData> _lootboxesData;

    private int _lootboxCounter;
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;
    private bool _isAdFinish;

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
        AdsManager.Instance.OnAdsIsReady += AdsIsReady;
        ShowPrevNextButtons();
        ShowLootboxOpenButton();
        _moneyText.text = PlayerProgress.Instance.PlayerMoney.ToString();
        Show();
    }

    public void OnHide()
    {
        Close();
    }

    public void GoToLobbyUI()
    {
        UIManager.Instance.UIStackPop();
    }

    public void ChangeLootbox(int value)
    {
        _lootboxCounter += value;
        ShowPrevNextButtons();
        _lootboxInfoPanelUI.SetInfoPanel(_lootboxesData[_lootboxCounter]);
        ShowLootboxOpenButton();
        _lootPanelUI.InitLootbox(_lootboxesData[_lootboxCounter]);
    }

    public void ShowAd()
    {
        _isAdFinish = false;
        AdsManager.Instance.OnFinishAd += FinishAd;
        AdsManager.Instance.ShowRewardedVideo();
    }

    public void OpenLootbox()
    {
        if (PlayerProgress.Instance.CheckLootsAvailability(_lootboxesData[_lootboxCounter]))
        {
            if (PlayerProgress.Instance.PlayerMoney >= _lootboxesData[_lootboxCounter].Price)
            {
                AudioManager.Instance.PlayEffect(_moneyClip);
                PlayerProgress.Instance.PlayerMoney -= _lootboxesData[_lootboxCounter].Price;
                _moneyText.text = PlayerProgress.Instance.PlayerMoney.ToString();
                UIManager.Instance.UIStackPush(_lootPanelUI);
            }
            else
            {
                AudioManager.Instance.PlayEffect(_clickClip);
                UIManager.Instance.UIStackPush(_donationUI);
            }
        }
    }

    public void OpenDonationUI()
    {
        UIManager.Instance.UIStackPush(_donationUI);
    }

    private void Open()
    {
        AdsManager.Instance.OnAdsIsReady += AdsIsReady;
        _isPopAvailable = true;
        _isBackButtonEnabled = true;
        _lootboxCounter = 0;
        ShowPrevNextButtons();
        ShowLootboxOpenButton();
        _moneyText.text = PlayerProgress.Instance.PlayerMoney.ToString();
        ChangeLootbox(_lootboxCounter);
        Show();
    }

    private void Close()
    {
        AdsManager.Instance.OnAdsIsReady -= AdsIsReady;
        StopAllCoroutines();
        Hide();
    }

    private void ShowPrevNextButtons()
    {
        if (_lootboxCounter >= _lootboxesData.Count - 1)
        {
            _nextButton.HideImmediate();
            _prevButton.ShowImmediate();
            _lootboxCounter = _lootboxesData.Count - 1;
        }
        else if (_lootboxCounter <= 0)
        {
            _prevButton.HideImmediate();
            _nextButton.ShowImmediate();
            _lootboxCounter = 0;
        }
        else
        {
            _nextButton.ShowImmediate();
            _prevButton.ShowImmediate();
        }
    }

    

    private void ShowLootboxOpenButton()
    {
        _openLootboxButton.Hide();
        _showAdButton.Hide();
        _lootboxInfoText.Hide();

        if (_lootboxesData[_lootboxCounter].IsAdLootbox)
        {
            if (Advertisement.IsReady())
            {
                ShowUIByRewardsAvailability();
            }
            else
            {
                string adsUnavailableKey = "AdsUnavailable";
                SetLootboxInfoText(adsUnavailableKey);
            }
        }
        else
        {
            if (PlayerProgress.Instance.CheckLootsAvailability(_lootboxesData[_lootboxCounter]))
            {
                _openLootboxButton.Show();
            }    
            else
            {
                string allLootsReceivedKey = "AllLootsReceived";
                SetLootboxInfoText(allLootsReceivedKey);
            }              
        }
    }

    private void ShowUIByRewardsAvailability()
    {
        switch(_lootboxesData[_lootboxCounter].TypeOfLootbox)
        {
            case LootboxData.Type.Money:
                if (DailyRewardsManager.Instance.NumberOfMoneyRewards > 0)
                {
                    _showAdButton.Show();
                }
                else
                {
                    string key = "AllAdsRewardsReceived";
                    SetLootboxInfoText(key);
                }
                break;
            case LootboxData.Type.Buff:
                if (DailyRewardsManager.Instance.NumberOfAmplificationRewards > 0)
                {
                    _showAdButton.Show();
                }
                else
                {
                    string key = "AllAdsRewardsReceived";
                    SetLootboxInfoText(key);
                }
                break;
            case LootboxData.Type.ResetTime:
                if(DailyRewardsManager.Instance.IsTimeChanged)
                {
                    _showAdButton.Show();
                }
                else
                {
                    string key = "TimeHasNotBeenChanged";
                    SetLootboxInfoText(key);
                }
                break;
        }       
    }

    private void SetLootboxInfoText(string key)
    {
        _lootboxInfoText.GetComponent<LocalizedText>().SetLocalization(key);
        _lootboxInfoText.Show();
    }

    private void AdsIsReady()
    {
        ShowLootboxOpenButton();
    }

    private void FinishAd()
    {
        AdsManager.Instance.OnFinishAd -= FinishAd;
        if (!_isAdFinish)
        {
            _isAdFinish = true;
            switch (_lootboxesData[_lootboxCounter].TypeOfLootbox)
            {
                case LootboxData.Type.Money:
                    UIManager.Instance.UIStackPush(_lootPanelUI);
                    break;
                case LootboxData.Type.Buff:
                    UIManager.Instance.UIStackPush(_lootPanelUI);
                    break;
                case LootboxData.Type.ResetTime:
                    DailyRewardsManager.Instance.ResetAllRewardsTime();
                    DailyRewardsManager.Instance.IsTimeChanged = false;
                    ShowLootboxOpenButton();
                    break;
            }
        }
    }
}