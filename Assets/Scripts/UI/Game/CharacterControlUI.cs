using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterControlUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private RebornUI _rebornUI;
    [SerializeField] private GameShopUI _gameShopUI;
    [SerializeField] private ElementIndicatorUI _elementIndicatorUI;

    [Header("Bars")]
    [SerializeField] private BarUI _healthBar;
    [SerializeField] private BarUI _armorBar;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    private Character _character;
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
        Close();
    }

    public void Init(Character character)
    {
        _healthBar.Init(character, character.MaxHealth, 0);
        _armorBar.Init(character, character.MaxArmor, 0);
        _character = character;
        _character.GetComponent<CharacterControl>().Joystick = _joystick;
        _gameShopUI.Init(_character);
        SubscribeToEvents();
    }

    public void Open()
    {
        _isPopAvailable = false;
        StopAllCoroutines();
        StartCoroutine(EnableBackButton());
        Show();
    }

    public void OpenPauseUI()
    {
        UIManager.Instance.UIStackPush(_pauseUI);
    }

    public void OpenRebornUI()
    {
        _rebornUI.Init(_character);
        UIManager.Instance.UIStackPush(_rebornUI);
    }

    public void OpenGameShopUI()
    {
        UIManager.Instance.UIStackPush(_gameShopUI);
    }

    private void SubscribeToEvents()
    {
        _character.OnCharacterDeath += OpenRebornUI;
        _character.OnMoneyChanged += SetMoneyText;
        _elementIndicatorUI.Init(_character.GetComponent<CharacterWeapon>());
    }

    private void Close()
    {
        _isBackButtonEnabled = false;
        Hide();
    }

    private void Update()
    {
        BackButtonClick();
    }

    private void SetMoneyText(int money)
    {
        _moneyText.text = money.ToString();
    }

    private void BackButtonClick()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isBackButtonEnabled 
            && UIManager.Instance.UIStackPeek() is CharacterControlUI)
        {
            OpenPauseUI();
        }
    }

    private IEnumerator EnableBackButton()
    {
        float enableBackButtonDelay = 0.25f;
        yield return new WaitForSecondsRealtime(enableBackButtonDelay);
        _isBackButtonEnabled = true;
    }
}
