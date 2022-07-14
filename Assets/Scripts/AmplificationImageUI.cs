using UnityEngine;
using UnityEngine.UI;

public class AmplificationImageUI : MonoBehaviour
{
    [SerializeField] private Image _amplificationImage;
    [SerializeField] private AnimationsUI _selectImage;
    
    private AmplificationData _amplificationData;
    private bool _isSelected;

    public AmplificationData AmplificationData => _amplificationData;
    
    public delegate void AmplificationSelected(AmplificationImageUI amplificationImageUI);

    public event AmplificationSelected OnAmplificationSelected;
    
    public void Init(AmplificationData amplificationData)
    {
        _amplificationData = amplificationData;
        _amplificationImage.sprite = _amplificationData.ItemSpriteUI;
    }

    public void SelectAmplification()
    {
        if (!_isSelected)
        {
            _selectImage.GetComponent<Image>().color = Color.white;
            _selectImage.Show();
            _isSelected = true;
            OnAmplificationSelected?.Invoke(this);
        }
    }

    public void RemoveAmplification()
    {
        _isSelected = false;
        _selectImage.Hide();
    }
}
