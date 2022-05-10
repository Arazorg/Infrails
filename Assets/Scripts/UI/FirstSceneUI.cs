using UnityEngine;

public class FirstSceneUI : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        GetComponentInChildren<AnimationsUI>().Show();
    }
}
