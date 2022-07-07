using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpDamageText : MonoBehaviour
{
    private const string ShowPopUp = "ShowText";
    private const string HidePopUp = "HideText";

    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Animator _animator;
    [SerializeField] private Color _x2DamageColor;

    private Enemy _enemy;
    private Vector3 _offset;
    private float _startSize;
    private float _currentPopUpDamage;
    private float _popUpTextFinishTime;

    public void SetText(string text)
    {
        _text.text = text;
        _text.colorGradient = new VertexGradient(Color.white);
        _text.color = _x2DamageColor;
        _animator.Play(ShowPopUp);
    }

    private void Start()
    {
        _startSize = _text.fontSize;
        _enemy = GetComponentInParent<Enemy>();
        _enemy.OnEnemyDamage += ShowPopUpText;
        _offset = _enemy.Data.PopUpTextOffset;    
    }

    private void ShowPopUpText(int damage)
    {
        float showTime = 1.25f;
        _popUpTextFinishTime = Time.time + showTime;
        _text.fontSize = _startSize / Mathf.Abs(transform.parent.localScale.x);
        transform.localPosition = _offset;

        if (_currentPopUpDamage == 0)
        {
            _animator.Play(ShowPopUp);
            StartCoroutine(ShowingPopUpText());
        }

        _currentPopUpDamage += damage;
        _text.text = _currentPopUpDamage.ToString();
    }

    private IEnumerator ShowingPopUpText()
    {
        while (Time.time < _popUpTextFinishTime)
            yield return null;

        _animator.Play(HidePopUp);
        _currentPopUpDamage = 0;
        _popUpTextFinishTime = 0;
    }

    private void Update()
    {
        SetScale();
    }

    private void SetScale()
    {
        if (transform.parent.localScale.x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
        else if (transform.parent.localScale.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, 1);
    }
}
