using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    private const string GameName = "INFRAILS";
    private const string AnyReferenceText = "ANY REFERENCE TO LIVING PERSONS OR REAL EVENTS IS PURELY COINCIDENTAL";
    private const string LoadingKey = "Loading";
    private const string HintKey = "LoadingHint";
    private const int CountOfHints = 5;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _sideText;

    [Header("Images")]
    [SerializeField] private Image _trolleyImage;
    [SerializeField] private Image _characterImage;
    [SerializeField] private Image _weaponImage;

    [Header("Sprites")]
    [SerializeField] private Sprite _defaultTrolleySprite;
    [SerializeField] private Sprite _trolleyForSupportSprite;

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        if (CurrentGameInfo.Instance == null)
        {
            SetTrolleySprite();
            _titleText.text = GameName;
            _sideText.text = AnyReferenceText;
        }
        else
        {
            _titleText.GetComponent<LocalizedText>().SetLocalization(LoadingKey);
            int numberOfHint = Random.Range(1, CountOfHints);
            _sideText.GetComponent<LocalizedText>().SetLocalization(string.Format("{0}{1}", HintKey, numberOfHint));

            if (CurrentGameInfo.Instance != null)
            {
                _characterImage.color = Color.white;
                _weaponImage.color = Color.white;
                _trolleyImage.sprite = CurrentGameInfo.Instance.TrolleyData.ItemSpriteUI;
                _characterImage.sprite = CurrentGameInfo.Instance.CharacterData.CharacterSprite;
                _weaponImage.sprite = CurrentGameInfo.Instance.CharacterData.CharacterStartWeapon.MainSprite;
            }
        }
    }

    private void SetTrolleySprite()
    {
        if (GetTrolleyForSupportAvailability())
            _trolleyImage.sprite = _trolleyForSupportSprite;
        else
            _trolleyImage.sprite = _defaultTrolleySprite;
    }


    private bool GetTrolleyForSupportAvailability()
    {
        string playerProgressFile = "PlayerProgress.txt";
        if (File.Exists($"{Application.persistentDataPath}/{playerProgressFile}"))
        {
            int trolleyAvailability = PlayerPrefs.GetInt(GameConstants.TrolleyForSupportKey, 0);
            if (trolleyAvailability == 1)
                return true;
            else
                return false;
        }
        else
        {
            PlayerPrefs.DeleteAll();
            return false;
        }
    }
}

