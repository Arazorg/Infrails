using UnityEngine;

public class BaseUI : MonoBehaviour
{
    protected Canvas ParentCanvas;

    [Header("Background Parameters")]
    [SerializeField] protected AnimationsUI Background;
    [SerializeField] protected float BackgroundAlpha;
    [SerializeField] private BackgroundType _backgroundType;

    private CanvasGroup _canvasGroup;

    public enum BackgroundType
    {
        Blur,
        Fade,
        Both,
        None
    }

    public void Show()
    {
        ParentCanvas.enabled = true;
        SetUIState(true);
        SetBackgroundState(true);

        foreach (var elementOfUI in GetComponentsInChildren<AnimationsUI>())
        {
            if (elementOfUI.IsShowOnStart)
                elementOfUI.Show();
        }
    }

    public void Hide()
    {
        foreach (var elementOfUI in GetComponentsInChildren<AnimationsUI>())
            elementOfUI.Hide();

        SetBackgroundState(false);
        SetUIState(false);
        ParentCanvas.enabled = false;
    }

    private void Awake()
    {
        ParentCanvas = GetComponentInParent<Canvas>();
        ParentCanvas.enabled = false;
    }

    private void SetUIState(bool isState)
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        _canvasGroup.blocksRaycasts = isState;
        if (isState)
            _canvasGroup.LeanAlpha(1, 0).setIgnoreTimeScale(true);
        else
            _canvasGroup.LeanAlpha(0, 0).setIgnoreTimeScale(true);
    }

    private void SetBackgroundState(bool isState)
    {
        switch (_backgroundType)
        {
            case BackgroundType.Blur:
                SetBlurBackground(isState);
                break;
            case BackgroundType.Fade:
                SetFadeBackground(isState);
                break;
            case BackgroundType.Both:
                SetBlurBackground(isState);
                SetFadeBackground(isState);
                break;
        }
    }

    private void SetBlurBackground(bool isState)
    {
        //GlobalVolumeManager.Instance.SetDepthOfFieldState(isState);
    }

    private void SetFadeBackground(bool isState)
    {
        if (isState)
            Background.SetTransparencyImmediate(BackgroundAlpha);
        else
            Background.SetTransparencyImmediate(0);
    }
}
