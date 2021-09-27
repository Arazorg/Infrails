using UnityEngine;
using UnityEngine.EventSystems;

public class LobbyCat : MonoBehaviour, IPointerDownHandler, IClickable
{
    private bool _isClickable;

    public bool IsClickable { get => _isClickable; set => _isClickable = value; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isClickable)
            LobbyEnvironmentManager.Instance.OpenSupportUsUI();
    }
}
