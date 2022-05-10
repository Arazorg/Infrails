using UnityEngine;

public class LobbyStopPoint : MonoBehaviour
{
    [SerializeField] private TutorialUI _tutorialUI;
    [SerializeField] private bool _isGoToGame;
    [SerializeField] private bool _isSetCharactersClickable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Trolley trolley))
        {
            if (_isGoToGame)
            {
                if (PlayerProgress.Instance.IsGameTutorialCompleted)
                    Loader.Load(Loader.Scene.Game);
                else
                    Loader.Load(Loader.Scene.Tutorial);
            }
            else if (_isSetCharactersClickable)
            {
                if (PlayerProgress.Instance.IsLobbyTutorialCompleted)
                    StartCoroutine(LobbyEnvironmentManager.Instance.SetLobbyObjectsСlickability(true));
                else
                    UIManager.Instance.UIStackPush(_tutorialUI);
            }
        }
    }
}
