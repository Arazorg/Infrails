using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterControlUI : BaseUI, IUIPanel
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private RebornUI _rebornUI;
    [SerializeField] private BarUI _healthBar;
    [SerializeField] private BarUI _armorBar;
    [SerializeField] private TextMeshProUGUI _moneyText;

    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }

    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    private Character _character;

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
        character.GetComponent<CharacterControl>().Joystick = _joystick;
        _character = character;
        character.OnCharacterDeath += OpenRebornUI;
        character.OnMoneyChanged += SetMoneyText;
    }

    public void Open()
    {
        _isPopAvailable = false;
        StopAllCoroutines();
        StartCoroutine(EnableBackButton());
        Show();
    }

    public void GetDamage()
    {
        _character.Damage(3);
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
