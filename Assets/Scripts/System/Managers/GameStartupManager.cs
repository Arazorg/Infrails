using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartupManager : MonoBehaviour
{
    [SerializeField] private TutorialUI _tutorialUI;
    [SerializeField] private GameStartUI _gameStartUI;
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
        LevelSpawner.Instance.StartSpawn();
        LevelSpawner.Instance.InitLevelUI();
        
        if (!PlayerProgress.Instance.IsGameTutorialCompleted)
        {
            UIManager.Instance.UIStackPush(_tutorialUI);
            _tutorialUI.OnTutorialFinish += StartGame;
        }
        else
        {
            Debug.Log("!");
            UIManager.Instance.UIStackPush(_gameStartUI);
            _gameStartUI.OnAmplificationsSelected += SetAmplifications;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void SetAmplifications(List<AmplificationData> amplificationsData)
    {
        _selectedAmplificationsData = new List<AmplificationData>();
        _selectedAmplificationsData = amplificationsData;
        StartGame();
        foreach (var amplificationData in _selectedAmplificationsData)
            _character.CharacterAmplifications.AddNewAmplification(amplificationData);
    }

    private void StartGame()
    {
        UIManager.Instance.UIStackPop();
        _tutorialUI.OnTutorialFinish -= StartGame;
        _gameStartUI.OnAmplificationsSelected -= SetAmplifications;
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
