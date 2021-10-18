using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialUI : BaseUI, IUIPanel
{
    private const string LobbySceneName = "Lobby";
    private const string GameSceneName = "Game";

    [SerializeField] private AnimationsUI _tutorialText;
    [SerializeField] private AnimationsUI _phraseImage;
    [SerializeField] private AnimationsUI _finishButton;
    [SerializeField] private Animator _guideAnimator;

    private List<TutorialItem> _allTutorialItems = new List<TutorialItem>();
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;
    private bool _isPhraseSpeedUp;

    public delegate void TutorialFinish();

    public event TutorialFinish OnTutorialFinish;

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
        Show();
    }

    public void OnHide()
    {
        Close();
    }

    public void SpeedUpPhrase()
    {
        _isPhraseSpeedUp = true;
        _tutorialText.SetPrintLetterDelay(0f);
    }

    public void FinishTutorial()
    {
        int lobbyFinishPhraseNumber = 7;
        int gameFinishPhraseNumber = 20;

        StopAllCoroutines();
        _phraseImage.Hide();
        _finishButton.Hide();
        switch (SceneManager.GetActiveScene().name)
        {
            case LobbySceneName:
                StartCoroutine(TutorialTextPrinting(lobbyFinishPhraseNumber, lobbyFinishPhraseNumber));
                PlayerProgress.Instance.SetLobbyTutorialComplete();
                break;
            case GameSceneName:
                StartCoroutine(TutorialTextPrinting(gameFinishPhraseNumber, gameFinishPhraseNumber));
                PlayerProgress.Instance.SetGameTutorialComplete();
                break;
        }
    }

    private void Start()
    {
        GetAllTutorialItems();
    }

    private void GetAllTutorialItems()
    {
        string tutorialItemsPath = "Specifications/TutorialItems";
        _allTutorialItems = Resources.LoadAll<TutorialItem>(tutorialItemsPath).ToList();
    }

    private void Open()
    {
        _isBackButtonEnabled = false;
        Show();
        ChooseTutorial();
    }

    private void Close()
    {
        StopAllCoroutines();
        Hide();
    }

    private void ChooseTutorial()
    {
        int lobbyStartIndex = 0;
        int lobbyFinishIndex = 6;
        int gameStartIndex = 8;
        int gameFinishIndex = 19;

        switch (SceneManager.GetActiveScene().name)
        {
            case LobbySceneName:
                StartCoroutine(TutorialTextPrinting(lobbyStartIndex, lobbyFinishIndex));
                break;
            case GameSceneName:
                StartCoroutine(TutorialTextPrinting(gameStartIndex, gameFinishIndex));
                break;
        }
    }

    private IEnumerator TutorialTextPrinting(int startNumberOfPhrase, int finishNumberOfPhrase)
    {
        float startDelay = 0.33f;
        float delayBetweenPhrases = 2f;
        float delayBetweenPhrasesSpeedUp = 0.5f;
        string animatorKey = "isSpeaking";
        float timeOfTyping = 1.5f;

        yield return new WaitForSeconds(startDelay);

        for (int i = startNumberOfPhrase; i <= finishNumberOfPhrase; i++)
        {
            _isPhraseSpeedUp = false;
            string printableText = LocalizationManager.GetLocalizedText(_allTutorialItems[i].PhraseKey);
            _guideAnimator.SetBool(animatorKey, true);
            yield return StartCoroutine(_tutorialText.TypingText(printableText, timeOfTyping));
            _guideAnimator.SetBool(animatorKey, false);

            if (_allTutorialItems[i].PhraseSprite == null)
            {
                if (_isPhraseSpeedUp)
                    yield return new WaitForSeconds(delayBetweenPhrasesSpeedUp);
                else
                    yield return new WaitForSeconds(delayBetweenPhrases);
            }
            else
            {
                yield return StartCoroutine(ShowPhraseImage(i, _allTutorialItems[i].ShowTime));
            }
        }

        PlayerProgress.Instance.SetLobbyTutorialComplete();
        UIManager.Instance.UIStackPop();
        OnTutorialFinish?.Invoke();
    }

    private IEnumerator ShowPhraseImage(int numberOfPhrase, float showTime, bool isBreak = false)
    {
        _phraseImage.GetComponent<Image>().sprite = _allTutorialItems[numberOfPhrase].PhraseSprite;
        _phraseImage.GetComponent<RectTransform>().sizeDelta = _allTutorialItems[numberOfPhrase].PhraseImageSize;
        _phraseImage.Show();
        yield return new WaitForSeconds(showTime);
        _phraseImage.Hide();
    }
}