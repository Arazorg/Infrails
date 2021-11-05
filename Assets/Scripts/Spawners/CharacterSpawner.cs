using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner Instance;

    private const int LobbyTrolleySpeed = 55;

    [Header("Prefabs")]
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private GameObject _trolleyPrefab;

    [Header("Trolleys data")]
    [SerializeField] private TrolleyData _defaultTrolleyData;
    [SerializeField] private TrolleyData _trolleyForSupportData;

    [Header("Camera")]
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private Vector3 _gameCameraOffset;
    [SerializeField] private float _gameCameraSize;

    public Transform SpawnTrolleyLobby(Transform spawnPoint)
    {
        Transform trolley = SpawnTrolley(spawnPoint);
        trolley.GetComponent<TrolleyMovement>().Speed = LobbyTrolleySpeed;
        CurrentGameInfo.Instance.TrolleyData = GetTrolleyData();
        return trolley;
    }

    public Transform SpawnTrolley(Transform spawnPoint)
    {
        Transform trolley = Instantiate(_trolleyPrefab, spawnPoint.position, Quaternion.identity).transform;
        trolley.GetComponent<Trolley>().Init(GetTrolleyData());
        return trolley;
    }

    public Character SpawnCharacter(CharacterData data, Transform spawnPoint)
    {
        var character = Instantiate(_characterPrefab, spawnPoint).GetComponent<Character>();
        character.Init(data);
        _cameraManager.Init();
        _cameraManager.SetCameraParams(character.Transform, _gameCameraSize, _gameCameraOffset);
        EnemiesManager.Instance.Character = character;
        return character;
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private TrolleyData GetTrolleyData()
    {
        if (PlayerProgress.Instance.GetTrolleyForSupportAvailability())
            return _trolleyForSupportData;
        else
            return _defaultTrolleyData;
    }
}
