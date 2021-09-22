using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private const string LobbySceneName = "Lobby";
    private const string GameSceneName = "Game";

    [SerializeField] private List<SafeArea> _safeAreas;
    [SerializeField] private BaseUI _startUI;

    private UIStack _stack;

    public void StartUI()
    {
        _stack = new UIStack();
        InitSafeAreas();
        GetUIForOpenByScene();
    }

    public void UIStackPush(IUIPanel panel)
    {
        _stack.Push(panel);
    }

    public void UIStackPop()
    {
        _stack.Pop();
    }

    public IUIPanel UIStackPeek()
    {
        return _stack.Peek();
    }

    private void Update()
    {
        BackButtonClick();
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

    private void BackButtonClick()
    {
        if(_stack.Count > 1)
        {
            IUIPanel currentPanel = _stack.Peek();
            if (Input.GetKeyDown(KeyCode.Escape) && currentPanel.IsPopAvailable && currentPanel.IsBackButtonEnabled)
            {
                UIStackPop();
            }
        }       
    }

    private void InitSafeAreas()
    {
        foreach (var safeArea in _safeAreas)
        {
            safeArea.Init();
        }
    }

    private void GetUIForOpenByScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case LobbySceneName:
                _stack.Push((LobbyUI)_startUI);
                break;
            case GameSceneName:
                _stack.Push((CharacterControlUI)_startUI);
                break;
        }
    }
}
