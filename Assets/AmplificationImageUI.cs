using UnityEngine;
using UnityEngine.UI;

public class AmplificationImageUI : MonoBehaviour
{
    [SerializeField] private Image _amplificationImage;
    [SerializeField] private AnimationsUI _selectImage;
    
    private AmplificationData _amplificationData;
    private bool _isSelected;

    public AmplificationData AmplificationData => _amplificationData;
    
    public delegate void AmplificationClick(AmplificationImageUI amplificationImageUI);

    public event AmplificationClick OnAmplificationClick;
    
    public void Init(AmplificationData amplificationData)
    {
        _amplificationData = amplificationData;
        _amplificationImage.sprite = _amplificationData.ItemSpriteUI;
    }

    public void Click()
    {
        if (!_isSelected)
        {
            _selectImage.GetComponent<Image>().color = Color.white;
            _selectImage.Show();
            _isSelected = true;
            OnAmplificationClick?.Invoke(this);
        }
    }

    public void RemoveAmplification()
    {
        _isSelected = false;
        _selectImage.Hide();
    }
}
