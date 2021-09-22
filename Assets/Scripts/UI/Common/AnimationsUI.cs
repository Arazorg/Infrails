﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationsUI : MonoBehaviour
{
    [SerializeField] private Vector2 _startPosition;
    [SerializeField] private Vector2 _finishPosition;

    [SerializeField] private bool _isShowOnStart;
    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isResizing;
    [SerializeField] private bool _isBlink;

    [SerializeField] private float _timeOfMovement = 0.33f;
    [SerializeField] private float _timeOfScaling = 0.33f;
    [SerializeField] private float _timeOfFading = 0.33f;
    [SerializeField] private float _timeOfBlink = 0.33f;

    private RectTransform _rectTransform;
    private TextMeshProUGUI _text;
    private float _letterPrintDelay;

    public bool IsShowOnStart
    {
        get { return _isShowOnStart; }
    }

    public void Show()
    {
        if (_isMoving)
        {
            MoveToPosition(_finishPosition);
        }
        else if (_isResizing)
        {
            transform.LeanScale(new Vector3(1, 1, 1), _timeOfScaling).setIgnoreTimeScale(true);
        }
    }

    public void ShowImmediate()
    {
        if (_isMoving)
        {
            if(_rectTransform == null)
            {               
                SetStartParams();
            }
            MoveToPosition(_finishPosition);
            _rectTransform.anchoredPosition = _finishPosition;
        }
    }

    public void Hide()
    {
        if (_isMoving)
        {
            MoveToPosition(_startPosition);
        }
        else if (_isResizing)
        {
            transform.LeanScale(Vector3.zero, _timeOfScaling).setIgnoreTimeScale(true);
        }
    }

    public void HideImmediate()
    {
        if (_isMoving)
        {
            if (_rectTransform == null)
            {
                SetStartParams();
            }
            MoveToPosition(_startPosition);
            _rectTransform.anchoredPosition = _startPosition;
        }
        else if (_isResizing)
        {
            transform.LeanScale(Vector3.zero, 0).setIgnoreTimeScale(true);
        }
    }

    public void SetTransparency(float alpha)
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        if (alpha == 0)
        {
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.blocksRaycasts = true;
        }

        StartCoroutine(Fading(alpha));
    }

    public void SetTransparencyImmediate(float alpha)
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        if (alpha == 0)
        {
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.blocksRaycasts = true;
        }

        canvasGroup.LeanAlpha(alpha, 0).setIgnoreTimeScale(true);
    }

    public IEnumerator TypingText(string printableText, float timeOfTyping)
    {
        _text.text = "";
        _letterPrintDelay = timeOfTyping / printableText.Length;

        float maxletterDelay = 0.05f;
        float minletterDelay = 0.02f;

        if (_letterPrintDelay > maxletterDelay)
        {
            _letterPrintDelay = maxletterDelay;
        }
        else if (_letterPrintDelay < minletterDelay)
        {
            _letterPrintDelay = minletterDelay;
        }

        foreach (char letter in printableText.ToCharArray())
        {
            _text.text += letter;
            yield return new WaitForSeconds(_letterPrintDelay);
        }
    }

    public void SetPrintLetterDelay(float letterPrintDelay)
    {
        _letterPrintDelay = letterPrintDelay;
    }

    public void ClearText()
    {
        _text.text = "";
    }

    private void Start()
    {
        SetStartParams();
    }

    private void MoveToPosition(Vector3 position)
    {
        if (_rectTransform == null)
        {
            SetStartParams();
        }
        LeanTween.moveX(_rectTransform, position.x, _timeOfMovement).setEaseOutQuart().setIgnoreTimeScale(true);
        LeanTween.moveY(_rectTransform, position.y, _timeOfMovement).setEaseOutQuart().setIgnoreTimeScale(true);
    }

    private void SetStartParams()
    {
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponent<TextMeshProUGUI>();
        if (_isBlink)
        {
            StartCoroutine(BlinkText(_timeOfBlink));
        }
    }

    private IEnumerator Fading(float alpha)
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.LeanAlpha(alpha, _timeOfFading).setIgnoreTimeScale(true);
        if (alpha == 0)
        {
            while (canvasGroup.alpha != 0)
            {
                yield return null;
            }
        }

        yield break;
    }

    private IEnumerator BlinkText(float duration)
    {
        while (true)
        {
            float time = 0;
            Color transparentColor = new Color(1, 1, 1, 0);
            Color startValue = _text.color;
            Color endValue = transparentColor;
            if (startValue.a == 0)
            {
                endValue = new Color(1, 1, 1, 1);
            }

            while (time < duration)
            {
                _text.color = Color.Lerp(startValue, endValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            _text.color = endValue;
        }
    }
}
