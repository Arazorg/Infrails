using UnityEngine;

public class SupportUsUI : BaseUI, IUIPanel
{
    private const string SupportUsRewardKey = "SupportUsReward";
    private const string SupportUsNotRewardKey = "SupportUsNotReward";

    [Header("UI Scripts")]
    [SerializeField] private ShopLootPanelUI _shopLootPanelUI;

    [Header("Data")]
    [SerializeField] private LootboxData _trolleysLootboxData;
    [SerializeField] private TrolleyData _trolleyForSupportData;

    [Header("Localized Texts")]
    [SerializeField] private LocalizedText _supportText;

    public delegate void GetTrolleyForSupport(TrolleyData trolleyData);

    public event GetTrolleyForSupport OnGetTrolleyForSupport;

    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }

    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    public void OnHide()
    {
        Hide();
    }

    public void OnPop()
    {
        Hide();
    }

    public void OnPush()
    {
        Open();
    }

    public void OnShow()
    {
        Open();
    }

    public void ShowReward()
    {
        PlayerProgress.Instance.OnSupportDonateComplete -= ShowReward;

        if (!PlayerProgress.Instance.GetTrolleyForSupportAvailability())
        {
            PlayerProgress.Instance.SetTrolleyForSupportAvailable();
            _shopLootPanelUI.InitLootbox(_trolleysLootboxData);
            UIManager.Instance.UIStackPush(_shopLootPanelUI);
            OnGetTrolleyForSupport?.Invoke(_trolleyForSupportData);
        }
    }

    public void Close()
    {
        UIManager.Instance.UIStackPop();
    }

    private void Open()
    {
        PlayerProgress.Instance.OnSupportDonateComplete += ShowReward;
        _isPopAvailable = true;
        _isBackButtonEnabled = true;
        SetSupportText();
        Show();
    }

    private void SetSupportText()
    {
        if (PlayerProgress.Instance.GetTrolleyForSupportAvailability())
            _supportText.SetLocalization(SupportUsNotRewardKey);
        else
            _supportText.SetLocalization(SupportUsRewardKey);
    }
}
