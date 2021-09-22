using UnityEngine;

public class FirstSceneUI : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<AnimationsUI>().Show();
    }
}
