using UnityEngine;

public class RailsProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform _trolley;
    [SerializeField] private Vector2 _neededFinishPosition;

    private Vector2 startPosition;
    private Vector2 finishPosition;
    private Vector2 rightCornerOfRailsAnchor = new Vector2(1, 0.5f);

    private void Start()
    {
        SetTrolleyParams();
    }

    private void SetTrolleyParams()
    {
        startPosition = _trolley.position;
        _trolley.anchorMin = rightCornerOfRailsAnchor;
        _trolley.anchorMax = rightCornerOfRailsAnchor;
        _trolley.position = startPosition;
        finishPosition = _neededFinishPosition;
    }

    private void Update()
    {
        TrolleyMovement();
    }

    private void TrolleyMovement()
    {
        _trolley.anchoredPosition = Vector3.Lerp(_trolley.anchoredPosition, finishPosition, 0.025f);
    }
}
