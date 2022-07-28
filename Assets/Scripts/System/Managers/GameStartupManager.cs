using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartupManager : MonoBehaviour
{
    [SerializeField] private TutorialUI _tutorialUI;
    [SerializeField] private GameStartUI _gameStartUI;
    [SerializeField] private CharacterControlUI _characterControlUI;
    [SerializeField] private Transform _trolleySpawnPoint;

    private List<AmplificationData> _selectedAmplificationsData;
    private Character _character;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioManager.Instance.StartAudio();
        UIManager.Instance.StartUI();

        if (CurrentGameInfo.Instance.IsInfinite)
            LevelSpawner.Instance.StartSpawnInfinite();
        else
            LevelSpawner.Instance.StartSpawn();

        if (!PlayerProgress.Instance.IsGameTutorialCompleted)
        {
            UIManager.Instance.UIStackPush(_tutorialUI);
            _tutorialUI.OnTutorialFinish += PushStartUI;
        }
        else
        {
            PushStartUI();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void PushStartUI()
    {
        if(CurrentGameInfo.Instance.IsInfinite)
        {
            UIManager.Instance.UIStackPush(_characterControlUI);
            StartGame();
        }
        else
        {
            UIManager.Instance.UIStackPush(_gameStartUI);
            _gameStartUI.OnStartGamePanelClosed += SetAmplificationsAndWeapon;
        }    
    }

    private void SetAmplificationsAndWeapon(List<AmplificationData> amplificationsData, WeaponData weaponData)
    {
        _selectedAmplificationsData = new List<AmplificationData>();
        _selectedAmplificationsData = amplificationsData;
        StartGame();
        foreach (var amplificationData in _selectedAmplificationsData)
            _character.CharacterAmplifications.AddNewAmplification(amplificationData);
        _character.CharacterWeapon.SpawnWeapon(weaponData);
    }

    private void StartGame()
    {
        UIManager.Instance.UIStackPop();
        _tutorialUI.OnTutorialFinish -= StartGame;
        _gameStartUI.OnStartGamePanelClosed -= SetAmplificationsAndWeapon;
        PlayerProgress.Instance.SetGameTutorialComplete();
        Transform trolley = CharacterSpawner.Instance.SpawnTrolley(_trolleySpawnPoint);
        _character = CharacterSpawner.Instance.SpawnCharacter(CurrentGameInfo.Instance.CharacterData, trolley);
        InitUI(_character);
        trolley.GetComponent<TrolleyMovement>().NextRail = LevelSpawner.Instance.CurrentBiomeStartRail;
        CurrentGameInfo.Instance.GameStartTime = Time.time;
    }

    private void InitUI(Character character)
    {
        CharacterControlUI characterControlUI = UIManager.Instance.UIStackPeek() as CharacterControlUI;
        characterControlUI.Init(character);
    }
}
