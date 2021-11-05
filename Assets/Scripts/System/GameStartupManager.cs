using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartupManager : MonoBehaviour
{
    [SerializeField] private TutorialUI _tutorialUI;
    [SerializeField] private Transform _trolleySpawnPoint;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioManager.Instance.StartAudio();
        UIManager.Instance.StartUI();
        LevelSpawner.Instance.StartSpawn();

        if (!PlayerProgress.Instance.IsGameTutorialCompleted)
        {
            UIManager.Instance.UIStackPush(_tutorialUI);
            _tutorialUI.OnTutorialFinish += StartGame;
        }
        else
        {
            StartGame();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void StartGame()
    {
        LevelSpawner.Instance.InitLevelUI();
        PlayerProgress.Instance.SetGameTutorialComplete();
        Transform trolley = CharacterSpawner.Instance.SpawnTrolley(_trolleySpawnPoint);
        Character character = CharacterSpawner.Instance.SpawnCharacter(CurrentGameInfo.Instance.CharacterData, trolley);
        InitUI(character);
        trolley.GetComponent<TrolleyMovement>().NextRail = LevelSpawner.Instance.CurrentBiomeStartRail;
        CurrentGameInfo.Instance.GameStartTime = Time.time;
    }

    private void InitUI(Character character)
    {
        CharacterControlUI characterControlUI = UIManager.Instance.UIStackPeek() as CharacterControlUI;
        characterControlUI.Init(character);
    }
}
