using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public static CharacterSpawner Instance;

    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private GameObject _trolleyPrefab;
    [SerializeField] private TrolleyData _lobbyTrolleyData;
    [SerializeField] private Vector3 _gameCameraOffset;
    [SerializeField] private float _gameCameraSize;

    public Transform SpawnTrolley(TrolleyData trolleyData, Transform spawnPoint)
    {
        Transform trolley = Instantiate(_trolleyPrefab, spawnPoint.position, Quaternion.identity).transform;
        trolley.GetComponent<Trolley>().Init(trolleyData);
        return trolley;
    }

    public GameObject SpawnCharacter(CharacterData data, Transform spawnPoint)
    {
        GameObject characterObject = Instantiate(_characterPrefab, spawnPoint);
        var character = characterObject.GetComponent<Character>();
        character.Init(data);
        _cameraManager.Init();
        _cameraManager.SetCameraParams(characterObject.transform, _gameCameraSize, _gameCameraOffset);
        EnemySpawner.Instance.CharacterObject = characterObject;
        return characterObject;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
