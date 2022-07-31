using System;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] private GameObject _tiledRailsPrefab;
    [SerializeField] private Rail _nextRail;
    [SerializeField] private Sprite _horizontalRailSprite;

    [Header("Rail states")]
    [SerializeField] private bool _isLobby;
    [SerializeField] private bool _isFinish;
    [SerializeField] private bool _isSpawnEnemies;
    [SerializeField] private bool _isSpawnBiome;

    private Biome _currentBiome;

    public delegate void ReachedEndOfLevel();

    public event ReachedEndOfLevel OnReachedEndOfLevel;

    public Transform RailTransform => transform;

    public bool IsSpawnEnemies => _isSpawnEnemies;

    public Rail NextRail { get => _nextRail; set => _nextRail = value; }

    private void Start()
    {
        if (!_isLobby)
        {
            _currentBiome = GetComponentInParent<Biome>();
            SpawnTiledRails();
        }
    }

    private void SpawnTiledRails()
    {
        if (_nextRail != null)
        {
            var tiledRailsSpriteRenderer = Instantiate(_tiledRailsPrefab, transform).GetComponent<SpriteRenderer>();
            SetTiledRailsSize(tiledRailsSpriteRenderer);
        }
    }

    private void SetTiledRailsSize(SpriteRenderer tiledRailsSpriteRenderer)
    {
        float distanceBetweenRails = 2;

        Vector2 tiledRailsSize = Vector2.zero;
        Vector2 tiledRailPosition = Vector2.zero;

        if (_nextRail.transform.position.y == transform.position.y)
        {
            tiledRailsSize = new Vector2(Math.Abs(_nextRail.transform.position.x - transform.position.x) - distanceBetweenRails, 2);
            tiledRailPosition = new Vector2((_nextRail.transform.position.x + transform.position.x) / 2, _nextRail.transform.position.y);
            tiledRailsSpriteRenderer.sprite = _horizontalRailSprite;
        }
        else if (_nextRail.transform.position.x == transform.position.x)
        {
            tiledRailsSize = new Vector2(2, Math.Abs(_nextRail.transform.position.y - transform.position.y) - distanceBetweenRails);
            tiledRailPosition = new Vector2(_nextRail.transform.position.x, (_nextRail.transform.position.y + transform.position.y) / 2);
        }

        tiledRailsSpriteRenderer.size = tiledRailsSize;
        tiledRailsSpriteRenderer.transform.position = tiledRailPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Trolley trolley))
        {
            if (_isFinish && (LevelSpawner.Instance.IsBossPhase || _nextRail != null))
                _currentBiome.SetBiomeLightsState(GameConstants.TurnOff);
            else if (_isSpawnBiome)
                EnterFinishRail();
            else if (_isSpawnEnemies && !LevelSpawner.Instance.IsBossPhase)
                EnemiesManager.Instance.SpawnFlyingEnemies(GetComponentInParent<RailsPattern>().FlyingEnemiesSpawnPoints);
        }
    }

    private void EnterFinishRail()
    {
        CurrentGameInfo.Instance.AddReachedBiome();
        OnReachedEndOfLevel?.Invoke();
    }
}
