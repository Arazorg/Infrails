using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameShopAdsUI : MonoBehaviour
{
    [Header("AnimationsUI")]
    [SerializeField] private AnimationsUI _moneyImage;
    [SerializeField] private AnimationsUI _plusMoneyImage;
    [SerializeField] private AnimationsUI _moneyAdsButton;
    [SerializeField] private AnimationsUI _noMoneyText;
    [SerializeField] private AnimationsUI _noAdsMoneyImage;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _adsMoneyText;
    [SerializeField] private TextMeshProUGUI _plusMoneyText;
    [SerializeField] private LocalizedText _noAdsText;

    private Character _character;
    private bool _isShowAd;
    private int _adsMoney;

    public void Init(Character character)
    {
        _character = character;
        _adsMoney = 30;
    }

    public void Open()
    {
        _adsMoneyText.text = _adsMoney.ToString();
    }

    public void AddMoneyForAds()
    {
        /*
        _isShowAd = true;
        AdsManager.Instance.OnFinishAd += AddMoney;
        if (Advertisement.IsReady())
        {
            AdsManager.Instance.ShowRewardedVideo();
        }
        else
        {
            StartCoroutine(ShowNoInternetImage());
        }
        */
    }

    public void StartAdsButtonJump()
    {
        StartCoroutine(MoneyAdsButtonJump());
    }

    public void StopAdsButtonJump()
    {
        _moneyImage.StopJump();
        _moneyAdsButton.StopJump();
    }

    private void AddMoney()
    {
        if (_isShowAd)
        {
            _isShowAd = false;
            _moneyImage.StartJump(new Vector2(75, 0));
            StartCoroutine(ShowPlusMoneyImage(_adsMoney));
            _character.AddMoney(_adsMoney);
            _adsMoney -= 6;
            _adsMoneyText.text = _adsMoney.ToString();
            if (_adsMoney == 0)
            {
                _noAdsMoneyImage.IsShowOnStart = true;
                _noAdsMoneyImage.Show();
                _moneyAdsButton.IsShowOnStart = false;
                _moneyAdsButton.Hide();
            }
        }
    }

    private IEnumerator MoneyAdsButtonJump()
    {
        Vector2 jumpOffset = new Vector2(0, 60);
        var jumpCoroutine = _moneyAdsButton.StartJump(jumpOffset);
        if (jumpCoroutine != null)
        {
            _noMoneyText.Show();
            yield return jumpCoroutine;
            _noMoneyText.Hide();
        }
    }

    private IEnumerator ShowNoInternetImage()
    {
        _noAdsText.SetLocalization("NoInternetAccess");
        _moneyAdsButton.Hide();
        _noAdsMoneyImage.Show();
        yield return new WaitForSecondsRealtime(2f);
        _noAdsText.SetLocalization("NoMoneyAds");
        _moneyAdsButton.Show();
        _noAdsMoneyImage.Hide();
    }

    private IEnumerator ShowPlusMoneyImage(int money)
    {
        yield return new WaitForSecondsRealtime(0.33f);
        _plusMoneyText.text = $"+{money}";
        _plusMoneyImage.Show();
        yield return new WaitForSecondsRealtime(1.5f);
        _plusMoneyImage.Hide();
    }
}
