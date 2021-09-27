using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private const string TrolleyTag = "Trolley";

    [SerializeField] private GameObject _tiledRailsPrefab;
    [SerializeField] private Rail _nextRail;
    [SerializeField] private Sprite _horizontalRailSprite;
    [SerializeField] private bool _isLobby;
    [SerializeField] private bool _isFinish;

    private Biome _currentBiome;

    public delegate void ReachedEndOfLevel();

    public event ReachedEndOfLevel OnReachedEndOfLevel;

    public Rail NextRail { get => _nextRail; set => _nextRail = value; }

    public Transform RailTransform => transform;

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

            Vector2 tiledRailsSize = Vector2.zero;
            Vector2 tiledRailPosition = Vector2.zero;

            float distanceBetweenRails = 2;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TrolleyTag))
        {
            if (_isFinish)
                EnterFinishRail();
        }
    }

    private void EnterFinishRail()
    {
        CurrentGameInfo.Instance.ReachedBiomeNumber++;
        EnemySpawner.Instance.MoveEnemiesToSpawnPoints();
        OnReachedEndOfLevel?.Invoke();
        _currentBiome.SetBiomeLightsState(GameConstants.TurnOff);
    }
}
