using UnityEngine;

public class FirstSceneUI : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 300;
        GetComponentInChildren<AnimationsUI>().Show();
    }
}
