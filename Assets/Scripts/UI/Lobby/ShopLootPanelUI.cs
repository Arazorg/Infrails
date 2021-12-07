using System.Collections;
using UnityEngine;

public class ShopLootPanelUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private DonationUI _donationUI;
    [SerializeField] private ShopLootsSender _shopLootsSender;
    [SerializeField] private LootInfoPanelUI _lootInfoPanelUI;
    [SerializeField] private LootboxUI _lootboxUI;

    [Header("Animations UI")]
    [SerializeField] private AnimationsUI _lootPanel;
    [SerializeField] private AnimationsUI _repeatOpenButton;
    [SerializeField] private AnimationsUI _topGoToShopUIButton;
    [SerializeField] private AnimationsUI _leftGoToShopUIButton;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _chestRotationClip;
    [SerializeField] private AudioClip _chestOpenClip;
    [SerializeField] private AudioClip _moneyClip;
    [SerializeField] private AudioClip _clickClip;

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
        Debug.Log(lootboxData.NameKey);
        _lootboxUI.Init(lootboxData);
    }

    public void Open()
    {
        ParentCanvas.enabled = true;
        _lootboxUI.OnAnimationEnd += ShowLoot;
        _lootboxUI.Show();
        Background.SetTransparencyImmediate(BackgroundAlpha);
        GlobalVolumeManager.Instance.SetVolumeProfile(true);
        AudioManager.Instance.PlayEffect(_chestRotationClip);
    }

    private void ShowLoot()
    {
        _lootboxUI.OnAnimationEnd -= ShowLoot;
        StartCoroutine(ShowingLoot());
    }

    private IEnumerator ShowingLoot()
    {
        AudioManager.Instance.PlayEffect(_chestOpenClip);
        float showButtonDelay = 1.5f;
        ItemData lootData = _shopLootsSender.GetLootData(_lootboxUI.LootboxData);
        if (lootData != null)
        {
            _lootboxUI.ResetLootbox();
            SetLootUI(lootData);
            yield return new WaitForSeconds(showButtonDelay);
            ShowButtons();
            _isPopAvailable = true;
            _isBackButtonEnabled = true;
        }
    }

    private void ShowButtons()
    {
        _repeatOpenButton.Hide();
        _leftGoToShopUIButton.Hide();
        _topGoToShopUIButton.Hide();

        bool lootsIsAvailable = PlayerProgress.Instance.CheckLootsAvailability(_lootboxUI.LootboxData);
        if (!_lootboxUI.LootboxData.IsAdLootbox && lootsIsAvailable)
        {
            _repeatOpenButton.Show();
            _leftGoToShopUIButton.Show();
            return;
        }

        _topGoToShopUIButton.Show();
    }

    public void RepeatOpen()
    {
        _isPopAvailable = false;
        _isBackButtonEnabled = false;
        Hide();

        if (PlayerProgress.Instance.CheckLootsAvailability(_lootboxUI.LootboxData))
        {
            if (PlayerProgress.Instance.PlayerMoney >= _lootboxUI.LootboxData.Price)
            {
                AudioManager.Instance.PlayEffect(_moneyClip);
                PlayerProgress.Instance.PlayerMoney -= _lootboxUI.LootboxData.Price;
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

    public void GoToShopUI()
    {
        UIManager.Instance.UIStackPop();
    }

    private void Close()
    {
        _isPopAvailable = true;
        _isBackButtonEnabled = false;
        StopAllCoroutines();
        Hide();
    }


    private void SetLootUI(ItemData lootData)
    {
        if(lootData is AmplificationData)
        {
            var data = lootData as AmplificationData;
            data.Level++;
        }

        _lootInfoPanelUI.SetInfoPanel(lootData);
        Show();
    }
}
