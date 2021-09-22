using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect safeArea;
    private Vector2 minAnchor;
    private Vector2 maxAnchor;
    private Vector2 currentResolution;


    public void Init()
    {
        currentResolution = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        SetSafeArea();
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void SetSafeArea()
    {
        safeArea = Screen.safeArea;
        minAnchor = safeArea.position;
        maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rectTransform.anchorMin = minAnchor;
        rectTransform.anchorMax = maxAnchor;
    }
}
