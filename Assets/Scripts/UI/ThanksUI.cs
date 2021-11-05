using System.Collections;
using UnityEngine;

public class ThanksUI : BaseUI, IUIPanel
{
    [SerializeField] private AnimationsUI _musicPanel;

    private Coroutine _showingCoroutine;
    private bool _isActive;
    private bool _isBackButtonEnabled;
    private bool _isPopAvailable;
    private bool _isFirstMusicPanel;

    public bool IsActive { get => _isActive; set => _isActive = value; }
    public bool IsBackButtonEnabled { get => _isBackButtonEnabled; set => _isBackButtonEnabled = value; }
    public bool IsPopAvailable { get => _isPopAvailable; set => _isPopAvailable = value; }

    public void OnHide()
    {
        Hide();
    }

    public void OnPop()
    {
        Hide();
    }

    public void OnPush()
    {
        Open();
    }

    public void OnShow()
    {
        Open();
    }

    public void Close()
    {
        StopCoroutine(_showingCoroutine);
        UIManager.Instance.UIStackPop();
    }

    public void OpenURL(string URL)
    {
        Application.OpenURL(URL);
    }

    private void Open()
    {
        _isPopAvailable = true;
        _isBackButtonEnabled = true;
        _isFirstMusicPanel = true;
        Show();
        _showingCoroutine = StartCoroutine(ShowingMusicPanels());
    }

    private IEnumerator ShowingMusicPanels()
    {
        float showingDuration = 3f;
        while(true)
        {
            if (_isFirstMusicPanel)
                _musicPanel.Hide();
            else
                _musicPanel.Show();
            _isFirstMusicPanel = !_isFirstMusicPanel;
            yield return new WaitForSeconds(showingDuration);
        }     
    }
}
