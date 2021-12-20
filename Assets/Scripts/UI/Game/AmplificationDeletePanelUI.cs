using UnityEngine;

public class AmplificationDeletePanelUI : MonoBehaviour
{
    [Header("UI Scripts")]
    [SerializeField] private CurrentAmplificationsPanelUI _currentAmplificationsPanelUI;

    public void DeleteAmplification(AmplificationData amplificationData)
    {
        _currentAmplificationsPanelUI.DeleteAmplification(amplificationData);
    }
}
