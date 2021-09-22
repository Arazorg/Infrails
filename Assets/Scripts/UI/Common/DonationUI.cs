using TMPro;
using UnityEngine;

public class DonationUI : BaseUI, IUIPanel
{
    [SerializeField] private TextMeshProUGUI _moneyText;

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
        Hide();
    }

    public void OnShow()
    {
        Open();
    }

    public void OnHide()
    {
        Hide();
    }

    public void Close()
    {
        PlayerProgress.Instance.OnMoneyUpdate -= UpdateMoney;
        UIManager.Instance.UIStackPop();
    }

    private void Open()
    {
        _isPopAvailable = true;
        _isBackButtonEnabled = true;
        _moneyText.text = PlayerProgress.Instance.PlayerMoney.ToString();
        PlayerProgress.Instance.OnMoneyUpdate += UpdateMoney;
        Show();
    }

    private void UpdateMoney()
    {
        _moneyText.text = PlayerProgress.Instance.PlayerMoney.ToString();
    }
}
