using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyEnvironmentManager : MonoBehaviour
{
    public static LobbyEnvironmentManager Instance;

    private const bool TeleportToTrolley = true;
    private const bool TeleportToStartPoint = false;

    [Header("UI scripts")]
    [SerializeField] private CharacterSelectionUI _characterSelectionUI;
    [SerializeField] private AvailableItemsUI _availableItemsUI;
    [SerializeField] private SupportUsUI _supportUsUI;

    [Header("Rails points")]
    [SerializeField] private Transform _trolleySpawnPoint;
    [SerializeField] private Rail _charZoneStopRail;
    [SerializeField] private Rail _goToGameRail;

    [Header("Camera parametrs")]
    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private Vector3 _selectionUICameraOffset;
    [SerializeField] private Vector3 _lobbyUICameraOffset;
    [SerializeField] private float _selectionUICameraSize;
    [SerializeField] private float _lobbyUICameraSize;

    private TrolleyMovement _trolleyMovement;
    private List<SelectableCharacter> _selectableCharacters = new List<SelectableCharacter>();
    private SelectableCharacter _currentSelectableCharacter;
    private bool isCharacterConfirmed;

    public bool IsCharacterConfirmed
    {
        get
        {
            return isCharacterConfirmed;
        }

        set
        {
            isCharacterConfirmed = value;
            MoveTrolleyToGame();
            SetCameraParams(GameConstants.CameraToCharacter, GameConstants.CloseCharacterSelectionUI);
        }
    }

    public void StartSpawn()
    {
        var trolley = CharacterSpawner.Instance.SpawnTrolleyLobby(_trolleySpawnPoint);
        _trolleyMovement = trolley.GetComponent<TrolleyMovement>();
        _trolleyMovement.NextRail = _charZoneStopRail;
        StartCoroutine(ShowingCharacters());
    }

    public void ShowCharacters()
    {
        StartCoroutine(ShowingCharacters());
    }

    public void SelectCharacter(SelectableCharacter currentSelectableCharacter)
    {
        _currentSelectableCharacter = currentSelectableCharacter;
        _currentSelectableCharacter.Teleport(TeleportToTrolley, _trolleyMovement.transform);
        SetCameraParams(GameConstants.CameraToCharacter, GameConstants.OpenCharacterSelectionUI);
        StartCoroutine(SetLobbyObjectsСlickability(false));
        _characterSelectionUI.SetCharacterData(currentSelectableCharacter.Data);
        UIManager.Instance.UIStackPush(_characterSelectionUI);
    }

    public void CancelCharacterSelection()
    {
        StartCoroutine(SetLobbyObjectsСlickability(true));
        _currentSelectableCharacter.Teleport(TeleportToStartPoint);
        SetCameraParams(GameConstants.CameraToLobby, GameConstants.CloseCharacterSelectionUI);
    }

    public IEnumerator SetLobbyObjectsСlickability(bool isClickable, float delay = 0.33f)
    {
        yield return new WaitForSeconds(delay);
        foreach (var clickableObject in GetComponentsInChildren(typeof(IClickable)))
            (clickableObject as IClickable).IsClickable = isClickable;        
    }

    public void MoveTrolleyToGame()
    {
        _trolleyMovement.NextRail = _goToGameRail;
    }

    public void OpenAvailableAmplificationsUI()
    {
        UIManager.Instance.UIStackPush(_availableItemsUI);
        _availableItemsUI.Init(AvailableItemsUI.TypeOfPanel.Amplification);
    }

    public void OpenAvailableWeaponsUI()
    {
        UIManager.Instance.UIStackPush(_availableItemsUI);
        _availableItemsUI.Init(AvailableItemsUI.TypeOfPanel.Weapon);
    }

    public void OpenSupportUsUI()
    {
        _supportUsUI.OnGetTrolleyForSupport += ChangeTrolley;
        UIManager.Instance.UIStackPush(_supportUsUI);
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void ChangeTrolley(TrolleyData trolleyData)
    {
        _supportUsUI.OnGetTrolleyForSupport -= ChangeTrolley;
        _trolleyMovement.GetComponent<Animator>().runtimeAnimatorController = trolleyData.AnimatorController;
        CurrentGameInfo.Instance.TrolleyData = trolleyData;
    }

    private void Start()
    {
        _cameraManager.Init();
    }

    private IEnumerator ShowingCharacters()
    {
        float startDelay = 0.2f;
        HideCharacters();
        yield return new WaitForSeconds(startDelay);
        float characterShowDelay = 0.2f;
        System.Random random = new System.Random();
        foreach (var character in _selectableCharacters.OrderBy(x => random.Next()))
        {
            character.SetCharacterVisiblity(true);
            yield return new WaitForSeconds(characterShowDelay);
        }
    }

    private void HideCharacters()
    {
        _selectableCharacters = GetComponentsInChildren<SelectableCharacter>().ToList();

        foreach (var character in _selectableCharacters)
            character.SetCharacterVisiblity(false);
    }

    private void SetCameraParams(bool toCharacter, bool isOpenSelectionUI)
    {
        if (toCharacter)
        {
            if (isOpenSelectionUI)
                _cameraManager.SetLobbyCameraParams(_charZoneStopRail.transform, _selectionUICameraSize, _selectionUICameraOffset);
            else
                _cameraManager.SetLobbyCameraParams(_trolleyMovement.transform, _lobbyUICameraSize, new Vector3(0, 0, -10));
        }
        else
        {
            _cameraManager.SetLobbyCameraParams(_trolleySpawnPoint, _lobbyUICameraSize, _lobbyUICameraOffset);
        }
    }
}
