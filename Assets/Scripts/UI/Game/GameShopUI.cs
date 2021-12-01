using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameShopUI : BaseUI, IUIPanel
{
    private const int HealPotionHealth = 5;

    [Header("UI Scripts")]
    [SerializeField] private GameShopProductPanel _gameShopProductPanel;
    [SerializeField] private GameShopAdsUI _gameShopAdsUI;
    [SerializeField] private BarUI _healthBar;
    [SerializeField] private BarUI _armorBar;
    [SerializeField] private ElementIndicatorUI _elementIndicatorUI;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    [Header("AnimationsUI")]
    [SerializeField] private AnimationsUI _moneyImage;
    [SerializeField] private AnimationsUI _prevButton;
    [SerializeField] private AnimationsUI _nextButton;

    [Header("Data")]
    [SerializeField] private List<GameShopProductData> _products;

    [Header("Animators")]
    [SerializeField] private Animator _healingEffectAnimator;

    private Character _character;
    private CharacterWeapon _characterWeapon;
    private CharacterAmplifications _characterAmplifications;

    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;
    private int _productsCounter;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }

    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    public void OnHide()
    {
        Close();
    }

    public void OnPop()
    {
        Close();
    }

    public void OnPush()
    {
        Open();
    }

    public void OnShow()
    {
        Open();
    }

    public void Init(Character character)
    {
        _character = character;
        _characterWeapon = character.GetComponent<CharacterWeapon>();
        _characterAmplifications = character.GetComponent<CharacterAmplifications>();
        _healthBar.Init(character, character.MaxHealth, 0);
        _armorBar.Init(character, character.MaxArmor, 0);
        _elementIndicatorUI.Init(character.GetComponent<CharacterWeapon>());
        _gameShopAdsUI.Init(character);
        character.OnMoneyChanged += SetMoneyText;
    }

    public void Open()
    {
        Time.timeScale = 0f;
        _isBackButtonEnabled = true;
        _isPopAvailable = true;
        _productsCounter = 0;
        SetProductInfo();
        Show();
    }

    public void GoToGame()
    {
        UIManager.Instance.UIStackPop();
    }

    public void SetItem(int step)
    {
        if (_productsCounter + step < _products.Count && _productsCounter + step >= 0)
            _productsCounter += step;

        if(step == -1)
            _nextButton.Show();
        else if(step == 1)
            _prevButton.Show();

        if (_productsCounter == _products.Count - 1)
            _nextButton.HideImmediate();

        if (_productsCounter == 0)
            _prevButton.HideImmediate();

        SetProductInfo();
    }

    public void BuyProduct()
    {
        if (_character.Money >= _products[_productsCounter].Price)
        {
            _character.AddMoney(-_products[_productsCounter].Price);
            switch (_products[_productsCounter].ProductType)
            {
                case GameShopProductData.Type.WeaponLootbox:
        
                    break;
                case GameShopProductData.Type.AmplificationLootbox:

                case GameShopProductData.Type.HealPotion:
                    _healingEffectAnimator.Play("Healing");
                    _character.Heal(HealPotionHealth);
                    break;
                case GameShopProductData.Type.ElementPotion:
                    Vector2 jumpOffset = new Vector2(0, -100);
                    _elementIndicatorUI.GetComponent<AnimationsUI>().StartJump(jumpOffset);
                    _characterWeapon.SetWeaponElement(LevelSpawner.Instance.CurrentBiomeData.OppositeBiomeElement);
                    break;
            }
        }
        else
        {
            _moneyImage.StartJump(new Vector2(75, 0));
            _gameShopAdsUI.StartAdsButtonJump();
        }
    }

    private void Close()
    {
        Time.timeScale = 1f;
        _isBackButtonEnabled = false;
        _gameShopAdsUI.StopAdsButtonJump();
        _moneyImage.StopJump();
        Hide();
    }

    private void SetProductInfo()
    {
        _gameShopProductPanel.SetInfo(_products[_productsCounter]);
    }

    private void SetMoneyText(int money)
    {
        _moneyText.text = money.ToString();
    }
}
