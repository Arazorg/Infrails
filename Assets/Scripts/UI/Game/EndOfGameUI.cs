using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfGameUI : BaseUI, IUIPanel
{
    [Header("Text")] [SerializeField] private TextMeshProUGUI _countOfEarnedMoneyText;
    [SerializeField] private TextMeshProUGUI _countOfKilledEnemyText;
    [SerializeField] private TextMeshProUGUI _playTimeText;
    [SerializeField] private TextMeshProUGUI _reachedLevelText;
    [SerializeField] private TextMeshProUGUI _doubleMoneyText;
    [SerializeField] private LocalizedText _resultText;

    [Header("Images")] [SerializeField] private Image _characterImage;
    [SerializeField] private Image _weaponImage;

    [Header("Rect Transform")] [SerializeField]
    private RectTransform _trolley;

    [SerializeField] private RectTransform _flag;

    [Header("AnimationsUI Scripts")] [SerializeField]
    private AnimationsUI _shareButton;

    [SerializeField] private AnimationsUI _doubleRewardButton;
    [SerializeField] private AnimationsUI _goToLobbyButton;
    [SerializeField] private AnimationsUI _doubleMoneyImage;
    [SerializeField] private AnimationsUI _goToNextLevelButton;
    [SerializeField] private AnimationsUI _retryLevelButton;

    [Header("Audio Clips")] [SerializeField]
    private AudioClip _defeatAudioClip;

    [SerializeField] private AudioClip _winAudioClip;

    private LevelData _levelData;
    private Vector2 _centerAnchor = new Vector2(0.5f, 0.5f);
    private Vector2 _startPosition;
    private Vector2 _finishPosition;
    private float _levelLength;
    private float _characterPositionY;
    private bool _isTrolleyMove;
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;
    private bool _isDoubleRewardAvailable = true;
    private bool _isWin;
    private bool _isInifinite;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public bool IsBackButtonEnabled
    {
        get => _isBackButtonEnabled;
        set => _isBackButtonEnabled = value;
    }

    public bool IsPopAvailable
    {
        get => _isPopAvailable;
        set => _isPopAvailable = value;
    }

    public void SetInfo(bool isWin, bool isInfinite, float characterPositionY = 0)
    {
        _isWin = isWin;
        _isInifinite = isInfinite;
        _characterPositionY = characterPositionY;
        SetResultText();
    }

    public void SetLevelInfo(LevelData levelData, float levelLength)
    {
        _levelData = levelData;
        _levelLength = levelLength;
    }

    public void OnPop()
    {
        GoToLobby();
    }

    public void OnPush()
    {
        StartCoroutine(Open());
    }

    public void OnShow()
    {
        StartCoroutine(Open());
    }

    public void OnHide()
    {
        Hide();
    }

    public void GoToLobby()
    {
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.Lobby);
    }

    public void Share()
    {
        StartCoroutine(Sharing());
    }

    public void GoToNextLevel()
    {
        Loader.Load(Loader.Scene.Game);
    }

    public void ShowAds()
    {
        /*
        AdsManager.Instance.OnFinishAd += DoubleReward;
        AdsManager.Instance.ShowRewardedVideo();
        */
    }

    private IEnumerator Open()
    {
        if (!CurrentGameInfo.Instance.IsInfinite)
            CurrentGameInfo.Instance.CountOfEarnedMoney += _levelData.LevelReward;
        
        CurrentGameInfo.Instance.AddResultsToProgress();
        _isPopAvailable = false;
        SetUI();
        Show();
        _doubleRewardButton.Show();
        float moveTrolleyDelay = 0.5f;
        yield return new WaitForSeconds(moveTrolleyDelay);
        SetTrolleyMovementParams();

        if (_isWin)
            AudioManager.Instance.PlayEffect(_winAudioClip);
        else
            AudioManager.Instance.PlayEffect(_defeatAudioClip);

        AudioManager.Instance.StopMusic();
        CurrentGameInfo.Instance.RefreshGameStats();
    }

    private void SetUI()
    {
        SetFlagPosition();
        SetText();
        SetFlag();
        SetCharacter();
        if (_isWin)
            _goToNextLevelButton.Show();
        else
            _retryLevelButton.Show();
    }

    private void SetFlagPosition()
    {
        float edgeOffset = Screen.safeArea.width / 9f; // indent = 11.1% of safeArea
        float startX = -((Screen.safeArea.width - (edgeOffset * 2)) / 2);

        if (_isInifinite)
        {
            float numberBiomeInGame = GameConstants.NumberBiomeInLevel * GameConstants.NumberLevelInGame;
            float oneBiomeOffset = (Screen.safeArea.width - (edgeOffset * 2)) / numberBiomeInGame;
            float finishX = (oneBiomeOffset * CurrentGameInfo.Instance.ReachedBiomeNumber);
            _finishPosition = new Vector2(startX + finishX, 0);
        }
        else
        {
            if (_isWin)
                _characterPositionY = _levelLength;

            float finishX = (Screen.safeArea.width - (edgeOffset * 2)) * (_characterPositionY / _levelLength);
            _finishPosition = new Vector2(startX + finishX, 0);
        }
    }

    private void SetResultText()
    {
        string winKey = "LevelWin";
        string loseKey = "LevelLose";
        string gameOverKey = "GameOver";

        if (_isInifinite)
        {
            _resultText.SetLocalization(gameOverKey);
        }
        else
        {
            if (_isWin)
                _resultText.SetLocalization(winKey);
            else
                _resultText.SetLocalization(loseKey);
        }
    }

    private void SetText()
    {
        float playTimeSeconds = (Time.time - CurrentGameInfo.Instance.GameStartTime);
        _playTimeText.text = string.Format("{0:00}:{1:00}", (int) (playTimeSeconds / 60), (int) (playTimeSeconds % 60));
        _countOfEarnedMoneyText.text = string.Format("+{0}", CurrentGameInfo.Instance.CountOfEarnedMoney.ToString());
        _doubleMoneyText.text = string.Format("+{0}", CurrentGameInfo.Instance.CountOfEarnedMoney.ToString());
        _countOfKilledEnemyText.text = CurrentGameInfo.Instance.CountOfKilledEnemies.ToString();
    }

    private void SetFlag()
    {
        if (_isInifinite)
        {
            int reachedLevel = CurrentGameInfo.Instance.ReachedBiomeNumber / GameConstants.NumberBiomeInLevel + 1;
            int reachedBiome = CurrentGameInfo.Instance.ReachedBiomeNumber % GameConstants.NumberBiomeInLevel;

            if (reachedBiome == 0)
            {
                reachedLevel--;
                reachedBiome = GameConstants.NumberBiomeInLevel;
            }

            _reachedLevelText.text = string.Format("{0}-{1}", reachedLevel, reachedBiome);
        }
        else
        {
            if (_isWin)
                _reachedLevelText.text = "100%";
            else
                _reachedLevelText.text = string.Format("{0}%", (int) ((_characterPositionY / _levelLength) * 100));
        }

        _flag.anchorMin = _centerAnchor;
        _flag.anchorMax = _centerAnchor;
        _flag.anchoredPosition = _finishPosition;

        if (_flag.anchoredPosition.x > 0)
        {
            _flag.localScale = new Vector3(-1, 1, 1);
            _reachedLevelText.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }
    }

    private void SetCharacter()
    {
        _characterImage.sprite = CurrentGameInfo.Instance.CharacterData.CharacterSprite;
        _weaponImage.sprite = CurrentGameInfo.Instance.CharacterData.CharacterStartWeapon.MainSprite;
    }

    private void SetTrolleyMovementParams()
    {
        _startPosition = _trolley.position;
        _trolley.anchorMin = _centerAnchor;
        _trolley.anchorMax = _centerAnchor;
        _trolley.position = _startPosition;
        _isTrolleyMove = true;
    }

    private void Update()
    {
        TrolleyMovement();
    }

    private void TrolleyMovement()
    {
        float t = 0.05f;
        if (_isTrolleyMove)
            _trolley.anchoredPosition = Vector3.Lerp(_trolley.anchoredPosition, _finishPosition, t);
    }

    private void DoubleReward()
    {
        if (_isDoubleRewardAvailable)
        {
            AdsManager.Instance.OnFinishAd -= DoubleReward;
            PlayerProgress.Instance.PlayerMoney += CurrentGameInfo.Instance.CountOfEarnedMoney;
            _countOfEarnedMoneyText.text = (CurrentGameInfo.Instance.CountOfEarnedMoney * 2).ToString();
            _isDoubleRewardAvailable = false;
            _doubleRewardButton.Hide();
            _doubleMoneyImage.Show();
        }
    }

    private IEnumerator Sharing()
    {
        _doubleRewardButton.HideImmediate();
        _doubleMoneyImage.HideImmediate();
        _goToLobbyButton.HideImmediate();
        _shareButton.HideImmediate();
        yield return _shareButton.GetComponent<ShareButtonUI>().Share();

        if (_isDoubleRewardAvailable)
            _doubleRewardButton.ShowImmediate();
        else
            _doubleMoneyImage.ShowImmediate();

        _goToLobbyButton.ShowImmediate();
        _shareButton.ShowImmediate();
    }
}