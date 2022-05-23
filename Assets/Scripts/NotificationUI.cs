using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private float _scaleDuration;
    [SerializeField] private Transform _textTransform;
    private Sequence _jumpingSequence;

    public void StopJumping()
    {
        transform.DOScale(0, _scaleDuration);
        StopAllCoroutines();
        _jumpingSequence.Kill();
    }

    private void Start()
    {
        CurrentGameInfo.Instance.OnReachedLevel += CheckLevelWeapon;
    }

    private void CheckLevelWeapon()
    {
        int levelNumberForStar = 3;
        int levelNumber = CurrentGameInfo.Instance.ReachedBiomeNumber;
        if (levelNumber % levelNumberForStar == 0)
            StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        yield return transform.DOScale(1, _scaleDuration).WaitForCompletion();
        yield return StartCoroutine(Jump());
        transform.DOScale(0, _scaleDuration);
    }

    private IEnumerator Jump()
    {
        int numberLoops = 15;
        float minSize = 0.8f;
        float maxSize = 1;
        float duration = 0.33f;

        _jumpingSequence = DOTween.Sequence();
        _jumpingSequence
            .Append(transform.DOScale(minSize, duration))
            .Append(transform.DOScale(maxSize, duration))
            .SetLoops(numberLoops);

        yield return _jumpingSequence.WaitForCompletion();
        yield break;
    }

    private void Update()
    {
        SetTextSize();
    }

    private void SetTextSize()
    {
        if (transform.localScale.x != 0)
        {
            float scale = 1f / transform.localScale.x;
            _textTransform.localScale = new Vector3(scale, scale, scale);
        }
    }
}