using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLootPanelUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private DonationUI _donationUI;
    [SerializeField] private WeaponInfoPanelUI _weaponInfoPanelUI;
    [SerializeField] private AmplificationInfoPanelUI _amplificationInfoPanelUI;
    [SerializeField] private LootInfoPanelUI _lootInfoPanelUI;
    [SerializeField] private LootboxUI _lootboxUI;

    [Header("Animations UI Scripts")]
    [SerializeField] private AnimationsUI _lootbox;
    [SerializeField] private AnimationsUI _lootPanel;
    [SerializeField] private AnimationsUI _weaponCharacteristicsPanel;
    [SerializeField] private AnimationsUI _amplificationInfoPanel;
    [SerializeField] private AnimationsUI _repeatOpenButton;
    [SerializeField] private AnimationsUI _topGoToShopUIButton;
    [SerializeField] private AnimationsUI _leftGoToShopUIButton;
    [SerializeField] private AnimationsUI _descriptionText;

    [Header("Data")]
    [SerializeField] private List<MoneyReward> _adMoneyRewards;
    [SerializeField] private ItemData _trolleyForSupportData;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _chestRotationClip;
    [SerializeField] private AudioClip _chestOpenClip;
    [SerializeField] private AudioClip _moneyClip;
    [SerializeField] private AudioClip _clickClip;

    private LootboxData _lootboxData;
    private Animator _lootboxAnimator;
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
        Show();
    }

    public void OnHide()
    {
        Close();
    }

    public void InitLootbox(LootboxData lootboxData)
    {
        _lootboxData = lootboxData;
        _lootbox.gameObject.SetActive(true);
        _lootboxUI.Init(lootboxData);
    }

    public void Open()
    {
        ParentCanvas.enabled = true;
        _lootboxUI.OnAnimationEnd += ShowLoot;
        _lootbox.gameObject.SetActive(true); 
        _lootbox.Show();
        _lootboxUI.Open();
        Background.SetTransparencyImmediate(BackgroundAlpha);
        GlobalVolumeManager.Instance.SetVolumeProfile(true);
        AudioManager.Instance.PlayEffect(_chestRotationClip);
    }

    public void ShowLoot()
    {
        StartCoroutine(ShowingLoot());
    }

    public IEnumerator ShowingLoot()
    {
        AudioManager.Instance.PlayEffect(_chestOpenClip);
        float showButtonDelay = 1.5f;
        ItemData lootData = GetRewardData();
        if (lootData != null)
        {
            ResetLootbox();
            SetLootUI(lootData);
            yield return new WaitForSeconds(showButtonDelay);

            _repeatOpenButton.Hide();
            _leftGoToShopUIButton.Hide();
            _topGoToShopUIButton.Hide();

            if (!_lootboxData.IsAdLootbox)
            {
                if (PlayerProgress.Instance.CheckLootsAvailability(_lootboxData))
                {
                    _repeatOpenButton.Show();
                    _leftGoToShopUIButton.Show();
                }
                else
                {
                    _topGoToShopUIButton.Show();
                }
            }
            else
            {
                _topGoToShopUIButton.Show();
            }

            _isPopAvailable = true;
            _isBackButtonEnabled = true;
        }
    }

    public void RepeatOpen()
    {
        _isPopAvailable = false;
        _isBackButtonEnabled = false;
        Hide();
        if (!_lootboxData.IsAdLootbox)
        {
            if (PlayerProgress.Instance.CheckLootsAvailability(_lootboxData))
            {
                if (PlayerProgress.Instance.PlayerMoney >= _lootboxData.Price)
                {
                    AudioManager.Instance.PlayEffect(_moneyClip);
                    PlayerProgress.Instance.PlayerMoney -= _lootboxData.Price;
                    Open();
                }
                else
                {
                    AudioManager.Instance.PlayEffect(_clickClip);
                    UIManager.Instance.UIStackPop();
                    UIManager.Instance.UIStackPush(_donationUI);
                }
            }
        }
    }

    public void GoToShopUI()
    {
        UIManager.Instance.UIStackPop();
    }

    private void Close()
    {
        _isPopAvailable = true;
        _isBackButtonEnabled = false;
        StopAllCoroutines();
        _lootPanel.SetTransparencyImmediate(0);
        Hide();
    }

    private void ResetLootbox()
    {
        _lootboxUI.ResetLootbox();
        _lootbox.gameObject.SetActive(false);
    }

    private ItemData GetRewardData()
    {
        var loots = PlayerProgress.Instance.GetLootsAvailableInShop(_lootboxData);
        switch (_lootboxData.TypeOfLootbox)
        {
            case LootboxData.Type.Weapon:
                var weaponData = loots[Random.Range(0, loots.Count)];
                PlayerProgress.Instance.SetWeaponAvailable(weaponData.ItemName);
                return weaponData;
            case LootboxData.Type.Skill:
                var skillData = loots[Random.Range(0, loots.Count)];
                PlayerProgress.Instance.SetPassiveSkillAvailable((skillData as PassiveSkillData).OwnerData.UnitName, skillData.ItemName);
                return skillData;
            case LootboxData.Type.Amplification:
                var amplificationData = loots[Random.Range(0, loots.Count)];
                PlayerProgress.Instance.IncrementAmplificationLevel(amplificationData.ItemName);
                return amplificationData;
            case LootboxData.Type.Money:
                return GetMoneyReward();
            case LootboxData.Type.Support:
                return _trolleyForSupportData;
        }

        return null;
    }

    private ItemData GetMoneyReward()
    {
        int number = DailyRewardsManager.Instance.NumberOfMoneyRewards;
        PlayerProgress.Instance.PlayerMoney += _adMoneyRewards[number - 1].Money;
        DailyRewardsManager.Instance.NumberOfMoneyRewards -= 1;
        return _adMoneyRewards[number - 1];
    }

    private void SetLootUI(ItemData lootData)
    {
        _lootPanel.SetTransparency(1);
        _lootInfoPanelUI.SetInfoPanel(lootData);
        Show();

        if (lootData is WeaponData)
            ShowWeaponLootInfo(lootData);
        else if (lootData is AmplificationData)
            ShowAmplificationsLootInfo(lootData);
        else
            _descriptionText.Show();
    }

    private void ShowWeaponLootInfo(ItemData lootData)
    {
        _weaponInfoPanelUI.SetPanelInfo(lootData);
        _weaponCharacteristicsPanel.Show();
        _descriptionText.Show();
    }

    private void ShowAmplificationsLootInfo(ItemData lootData)
    {
        var data = lootData as AmplificationData;
        data.Level++;
        _amplificationInfoPanelUI.SetPanelInfo(data);
        _amplificationInfoPanel.Show();
    }
}
