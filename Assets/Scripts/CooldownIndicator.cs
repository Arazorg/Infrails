using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownIndicator : MonoBehaviour
{
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _centerColor;
    [SerializeField] private Color _finishColor;
    [SerializeField] private TextMeshProUGUI _timeText;

    private Image _image;
    private float _startTime;
    private float _finishTime;

    public void SetFinishTime(float finishTime)
    {
        _image.fillAmount = 0;
        _startTime = Time.time;
        _finishTime = finishTime;
        StartCoroutine(FillImage());
    }


    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private IEnumerator FillImage()
    {
        while (Time.time < _finishTime)
        {
            float amount = 1 - ((_finishTime - Time.time)
                                / (_finishTime - _startTime));
            _image.fillAmount = amount;
            if (amount <= 0.5f)
                _image.color = Color.Lerp(_startColor, _centerColor, amount * 2);
            else
                _image.color = Color.Lerp(_centerColor, _finishColor, (amount - 0.5f) * 2);
            _timeText.text = string.Format("{0:f2}", (_finishTime - Time.time));
            yield return null;
        }
        _timeText.text = string.Empty;
        _image.fillAmount = 1;
    }
}
