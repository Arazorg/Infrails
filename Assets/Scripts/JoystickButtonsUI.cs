using UnityEngine;

public class JoystickButtonsUI : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CanvasGroup _buttons;

    private void Update()
    {
        if (_joystick.Horizontal > 0.5f || _joystick.Horizontal < -0.5f)
            _buttons.alpha = Mathf.Abs(_joystick.Horizontal);
    }
}
