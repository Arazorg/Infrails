using UnityEngine;

public class LobbyStopPoint : MonoBehaviour
{
    [SerializeField] private bool isGoToGame;
    [SerializeField] private bool isSetCharactersClickable;
    [SerializeField] private TutorialUI _tutorialUI;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Trolley"))
        {
            if (isGoToGame)
            {
                Loader.Load(Loader.Scene.Game);
            }
            else if(isSetCharactersClickable)
            {
                if(PlayerProgress.Instance.IsLobbyTutorialCompleted)
                {
                    LobbyEnvironmentManager.Instance.SetSelectableCharacterСlickability(true);
                }
                else
                {
                    UIManager.Instance.UIStackPush(_tutorialUI);
                }
            }
        }
    }
}
