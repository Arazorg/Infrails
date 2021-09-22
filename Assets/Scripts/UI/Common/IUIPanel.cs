using UnityEngine;

public interface IUIPanel
{
    public bool IsActive { get; set; }

    public bool IsBackButtonEnabled { get; set; }

    public bool IsPopAvailable  { get; set; }

    void OnPop();
    void OnPush();
    void OnShow();
    void OnHide();
}
