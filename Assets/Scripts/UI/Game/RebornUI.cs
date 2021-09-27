using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;

public class RebornUI : BaseUI, IUIPanel
{
    private const int CostOfResurrection = 200;

    [Header("UI Scripts")]
    [SerializeField] private DonationUI _donationUI;
    [SerializeField] private EndOfGameUI _endOfGameUI;

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private AnimationsUI _adsUnavailableText;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _moneyClip;
    [SerializeField] private AudioClip _clickClip;

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
        _moneyText.text = PlayerProgress.Instance.PlayerMoney.ToString();
        Show();
    }

    public void OnHide()
    {
        Close();
    }

    public void Close()
    {
        Time.timeScale = 1f;
        Hide();
    }

    public void Init(Character character)
    {
        _character = character;
    }

    public void RebornCharacterByMoney()
    {
        if (_character.IsCanReborn)
        {
            if(PlayerProgress.Instance.PlayerMoney >= CostOfResurrection)
            {
                AudioManager.Instance.PlayEffect(_moneyClip);
                PlayerProgress.Instance.PlayerMoney -= CostOfResurrection;
                Reborn();
            }
            else
            {
                AudioManager.Instance.PlayEffect(_clickClip);
                UIManager.Instance.UIStackPush(_donationUI);
            }
        }
    }

    public void RebornCharacterByAds()
    {
        AdsManager.Instance.OnFinishAd += Reborn;
        if (_character.IsCanReborn)
        {
            if (Advertisement.IsReady())
            {
                AdsManager.Instance.ShowRewardedVideo();
            }
            else
            {
                StartCoroutine(AdsUnavailableTextShowing());
            }
        }
    }

    public void GoToEndGameUI()
    {
        UIManager.Instance.UIStackPush(_endOfGameUI);
    }

    private void Open()
    {
        if (_character.IsCanReborn)
        {
            StartCoroutine(Opening());
        }
        else
        {
            StartCoroutine(OpeningEndGameUI());
        }
    }

    private IEnumerator Opening()
    {
        float startDelay = 0.75f;
        yield return new WaitForSeconds(startDelay);
        Time.timeScale = 0f;
        _moneyText.text = PlayerProgress.Instance.PlayerMoney.ToString();
        Show();
    }

    private IEnumerator OpeningEndGameUI()
    {
        float startDelay = 0.5f;
        yield return new WaitForSeconds(startDelay);
        GoToEndGameUI();
    }

    private void Reborn()
    {
        AdsManager.Instance.OnFinishAd -= Reborn;
        _character.Reborn();
        UIManager.Instance.UIStackPop();
    }

    private IEnumerator AdsUnavailableTextShowing()
    {
        float showDelay = 2f;
        _adsUnavailableText.Show();
        yield return new WaitForSecondsRealtime(showDelay);
        _adsUnavailableText.Hide();
    }
}