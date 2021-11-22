using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameShopUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private LevelInfoUI _levelInfoUI;
    [SerializeField] private ElementIndicatorUI _elementIndicatorUI;
    [SerializeField] private CharacterItemsPanelUI _characterItemsPanelUI;

    [Header("Bars")]
    [SerializeField] private BarUI _healthBar;
    [SerializeField] private BarUI _armorBar;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    [Header("Potions Data")]
    [SerializeField] private ItemData _healPotionData;
    [SerializeField] private ItemData _elementPotionData;

    private List<ItemData> _currentItemsData = new List<ItemData>();

    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;
    private int _itemsCounter;

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
        var characterWeapon = character.GetComponent<CharacterWeapon>();
        var characterAmplifications = character.GetComponent<CharacterAmplifications>();
        _healthBar.Init(character, character.MaxHealth, 0);
        _armorBar.Init(character, character.MaxArmor, 0);
        _elementIndicatorUI.Init(characterWeapon);
        _characterItemsPanelUI.SetItems(characterWeapon.CurrentWeapon.CurrentWeaponData,
                                            characterAmplifications.CurrentAmplificationsData);
        character.OnMoneyChanged += SetMoneyText;
        //LevelSpawner.Instance.OnBiomeSpawned += GetItem;
        //GetItem();
        //AddPotionData();
    }

    public void Open()
    {
        Time.timeScale = 0f;
        _isBackButtonEnabled = true;
        _isPopAvailable = true;
        _levelInfoUI.Hide();
        _itemsCounter = 0;
        Show();
    }

    public void NextItem()
    {
        if (_itemsCounter < _currentItemsData.Count - 1)
            _itemsCounter++;
    }

    public void PrevItem()
    {
        if (_itemsCounter > 0)
        {
            _itemsCounter--;
        }

    }

    private void Close()
    {
        Time.timeScale = 1f;
        _isBackButtonEnabled = false;
        Hide();
    }

    private void AddPotionData()
    {
        _currentItemsData[0] = _healPotionData;
        _currentItemsData[2] = _elementPotionData;
    }

    private void SetItemInfo(ItemData itemData)
    {
        
    }

    private void SetMoneyText(int money)
    {
        _moneyText.text = money.ToString();
    }

    private void GetItem()
    {
        if (Random.value < 0.66f)
        {
            _currentItemsData[1] = GetAmplificationData();
        }
        else
        {
            _currentItemsData[1] = GetWeaponData();
        }
    }

    private AmplificationData GetAmplificationData()
    {
        var amplificationsData = PlayerProgress.Instance.GetAmplificationsData(GameConstants.Available);
        var data = amplificationsData.Where(s => s.Level == LevelSpawner.Instance._itemsLevel).ToList();
        return data[Random.Range(0, data.Count)];
    }

    private WeaponData GetWeaponData()
    {
        var weaponsData = PlayerProgress.Instance.GetWeaponsData(GameConstants.Available);
        var data = weaponsData.Where(s => s.Level == LevelSpawner.Instance._itemsLevel).ToList();
        return data[Random.Range(0, data.Count)];
    }
}
