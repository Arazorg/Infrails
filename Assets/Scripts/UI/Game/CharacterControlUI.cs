using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControlUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PauseUI _pauseUI;
    [SerializeField] private RebornUI _rebornUI;

    [Header("Bars")]
    [SerializeField] private BarUI _healthBar;
    [SerializeField] private BarUI _armorBar;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    [Header("Images")]
    [SerializeField] private Image _elementOutlineImage;
    [SerializeField] private Image _elementImage;

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
        SubscribeToEvents();
        SetElementImage(LevelSpawner.Instance.CurrentBiomeData.BiomeElement);
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

    public void DamageCharacter()
    {
        _character.Damage(6);
    }

    private void SubscribeToEvents()
    {
        _character.OnCharacterDeath += OpenRebornUI;
        _character.OnMoneyChanged += SetMoneyText;
        _character.OnElementChanged += SetElementImage;
        LevelSpawner.Instance.OnBiomeSpawned += SetElementOutlineImage;
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

    private void SetElementImage(Element element)
    {
        _elementImage.sprite = element.ElementSpriteUI;
        SetElementOutlineImage();
    }

    private void SetElementOutlineImage()
    {
        _elementOutlineImage.color = Color.white;
        float interaction = LevelSpawner.Instance.CurrentBiomeData.BiomeElement.GetElementInteractionByType(_character.CurrentElement.ElementType);

        if (interaction < 1)
            _elementOutlineImage.color = Color.red;
        else if (interaction > 1)
            _elementOutlineImage.color = Color.green;
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
