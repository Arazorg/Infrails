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

    private List<TutorialItem> _lobbyItems = new List<TutorialItem>();
    private List<TutorialItem> _gameItems = new List<TutorialItem>();

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
        StopAllCoroutines();
        _phraseImage.Hide();
        _finishButton.Hide();
        switch (SceneManager.GetActiveScene().name)
        {
            case LobbySceneName:
                StartCoroutine(ShowLastPhrase(_lobbyItems[_lobbyItems.Count - 1]));
                PlayerProgress.Instance.SetLobbyTutorialComplete();
                break;
            case GameSceneName:
                StartCoroutine(ShowLastPhrase(_gameItems[_gameItems.Count - 1]));
                PlayerProgress.Instance.SetGameTutorialComplete();
                break;
        }
    }

    private void Open()
    {
        _isBackButtonEnabled = false;
        GetAllTutorialItems();
        Show();
        ChooseTutorial();
    }
    private void GetAllTutorialItems()
    {
        string tutorialLobbyItemsPath = "Specifications/TutorialItems/Lobby";
        string tutorialGameItemsPath = "Specifications/TutorialItems/Game";

        _lobbyItems = Resources.LoadAll<TutorialItem>(tutorialLobbyItemsPath).ToList();
        _gameItems = Resources.LoadAll<TutorialItem>(tutorialGameItemsPath).ToList();
    }

    private void Close()
    {
        StopAllCoroutines();
        Hide();
    }

    private void ChooseTutorial()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case LobbySceneName:
                StartCoroutine(TutorialTextPrinting(_lobbyItems));
                break;
            case GameSceneName:
                StartCoroutine(TutorialTextPrinting(_gameItems));
                break;
        }
    }

    private IEnumerator TutorialTextPrinting(List<TutorialItem> tutorialItems)
    {
        float startDelay = 0.33f;
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < tutorialItems.Count - 1; i++)
            yield return StartCoroutine(ShowPhrase(tutorialItems[i]));


        PlayerProgress.Instance.SetLobbyTutorialComplete();
        UIManager.Instance.UIStackPop();
        OnTutorialFinish?.Invoke();
    }

    private IEnumerator ShowPhrase(TutorialItem tutorialItem)
    {
        float timeOfTyping = 1.5f;
        float delayBetweenPhrases = 2f;
        float delayBetweenPhrasesSpeedUp = 0.5f;
        string animatorKey = "isSpeaking";

        _isPhraseSpeedUp = false;
        string printableText = LocalizationManager.GetLocalizedText(tutorialItem.PhraseKey);
        _guideAnimator.SetBool(animatorKey, true);
        yield return StartCoroutine(_tutorialText.TypingText(printableText, timeOfTyping));
        _guideAnimator.SetBool(animatorKey, false);

        if (tutorialItem.PhraseSprite == null)
        {
            if (_isPhraseSpeedUp)
                yield return new WaitForSeconds(delayBetweenPhrasesSpeedUp);
            else
                yield return new WaitForSeconds(delayBetweenPhrases);
        }
        else
        {
            yield return StartCoroutine(ShowPhraseImage(tutorialItem));
        }
    }

    private IEnumerator ShowPhraseImage(TutorialItem tutorialItem)
    {
        _phraseImage.GetComponent<Image>().sprite = tutorialItem.PhraseSprite;
        _phraseImage.GetComponent<RectTransform>().sizeDelta = tutorialItem.PhraseImageSize;
        _phraseImage.Show();
        yield return new WaitForSeconds(tutorialItem.ShowTime);
        _phraseImage.Hide();
    }

    private IEnumerator ShowLastPhrase(TutorialItem tutorialItem)
    {
        yield return StartCoroutine(ShowPhrase(tutorialItem));
        PlayerProgress.Instance.SetLobbyTutorialComplete();
        UIManager.Instance.UIStackPop();
        OnTutorialFinish?.Invoke();
    }
}