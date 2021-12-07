using System.Collections;
using UnityEngine;

public class GameShopLootUI : BaseUI, IUIPanel
{
    [Header("UI Scripts")]
    [SerializeField] private GameShopLootsSender _gameShopLootsSender;
    [SerializeField] private LootInfoPanelUI _lootInfoPanelUI;
    [SerializeField] private AmplificationsUI _amplificationsUI;
    [SerializeField] private GameShopWeaponStarsUI _gameShopWeaponStarsUI;
    [SerializeField] private LootboxUI _lootboxUI;

    [Header("Animations UI")]
    [SerializeField] private AnimationsUI _acceptButton;
    [SerializeField] private AnimationsUI _deleteButton;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _chestRotationClip;
    [SerializeField] private AudioClip _chestOpenClip;

    private CharacterWeapon _characterWeapon;
    private CharacterAmplifications _characterAmplifications;
    private ItemData _currentItem;
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
        GetComponentInParent<Canvas>().enabled = true;
        Background.SetTransparencyImmediate(BackgroundAlpha);
        GlobalVolumeManager.Instance.SetVolumeProfile(true);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<CanvasGroup>().alpha = 1;
    }

    public void OnHide()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().alpha = 0;
    }

    public void Init(Character character, GameShopProductData data)
    {
        _characterWeapon = character.GetComponent<CharacterWeapon>();
        _characterAmplifications = character.GetComponent<CharacterAmplifications>();
        _gameShopLootsSender.Init(_characterAmplifications);
        _lootboxUI.Init(data);
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
        ItemData lootData = _gameShopLootsSender.GetLootData(_lootboxUI.ProductData);
        _currentItem = lootData;
        if (lootData != null)
        {
            _lootboxUI.ResetLootbox();
            SetLootUI(lootData);
            yield return new WaitForSecondsRealtime(showButtonDelay);
            _acceptButton.Show();
            _deleteButton.Show();
            _isPopAvailable = true;
            _isBackButtonEnabled = true;
        }
    }

    public void DeleteItem()
    {
        UIManager.Instance.UIStackPop();
    }

    public void TakeItem()
    {
        if (_currentItem is WeaponData)
        {
            _characterWeapon.SpawnWeapon(_currentItem as WeaponData);
            UIManager.Instance.UIStackPop();
        }            
        else
        {
            if(!_characterAmplifications.AddNewAmplification(_currentItem as AmplificationData))
            {
                UIManager.Instance.UIStackPush(_amplificationsUI);
            }
            else
            {
                UIManager.Instance.UIStackPop();
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
        _lootInfoPanelUI.SetInfoPanel(lootData);
        Show();
        if (lootData is WeaponData)
        {
            _gameShopWeaponStarsUI.Show((lootData as WeaponData).StarsNumber);
        }
    }
}
