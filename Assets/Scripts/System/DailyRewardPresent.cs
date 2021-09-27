using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DailyRewardPresent : MonoBehaviour, IClickable
{
    private const string RewardRecievedAnimatorKey = "isRewardRecieved";
    private const string CanvasPopUpAnimatorKey = "PopUp";
    private const int RewardMoney = 250;

    [SerializeField] private Animator _canvasAnimator;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Animations UI Scripts")]
    [SerializeField] private AnimationsUI _moneyImage;
    [SerializeField] private AnimationsUI _timerImage;
    [SerializeField] private AnimationsUI _timeUnloadedImage;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _timerClip;
    [SerializeField] private AudioClip _rewardClip;

    private TimeSpan timeToReward;
    private Animator _animator;
    private bool _isRewardAvailable;
    private bool _isClickable;

    public bool IsClickable { get => _isClickable; set => _isClickable = value; }

    public void TryGetReward()
    {
        if(_isClickable)
        {
            _isClickable = false;
            StartCoroutine(SetClickable());
            HideInfoImages();
            GetRewardAvailablity();
            if (_isRewardAvailable)
                GetReward();
            else
                ShowTimeToReward();

            _animator.SetBool(RewardRecievedAnimatorKey, _isRewardAvailable);
            _canvasAnimator.Play(CanvasPopUpAnimatorKey);
        }     
    }

    private void Start()
    {
        timeToReward = new TimeSpan(0, GameConstants.HoursToDailyReward, 0, 0);
        _animator = GetComponent<Animator>();
        DailyRewardsManager.Instance.OnDailyRewardAvailable += GetRewardAvailablity;
        GetRewardAvailablity();
    }

    private void GetRewardAvailablity()
    {
        _isRewardAvailable = true;
        DateTime savedTime = DailyRewardsManager.Instance.GetDailyRewardSavedTime();

        if (DateTime.Now < savedTime)
        {
            _isRewardAvailable = false;
            if (savedTime.Ticks > timeToReward.Ticks)
            {
                if (DateTime.Now < savedTime - timeToReward)
                    DailyRewardsManager.Instance.IsTimeChanged = true;
            }
        }

        _animator.SetBool(RewardRecievedAnimatorKey, _isRewardAvailable);
    }

    private void GetReward()
    {
        _isRewardAvailable = false;
        _moneyImage.ShowImmediate();
        PlayerProgress.Instance.PlayerMoney += RewardMoney;
        SetNextRewardTime();
        AudioManager.Instance.PlayEffect(_rewardClip);
    }

    private void SetNextRewardTime()
    {
        DateTime nextRewardTime = DateTime.Now + timeToReward;
        DailyRewardsManager.Instance.SetDailyRewardTime(nextRewardTime.ToString());
    }

    private void ShowTimeToReward()
    {
        TimeSpan timeDifference = DailyRewardsManager.Instance.GetDailyRewardSavedTime() - DateTime.Now;
        DateTime timeToReward = new DateTime(timeDifference.Ticks);
        _timerText.text = timeToReward.ToString("HH:mm");
        _timerImage.ShowImmediate();
        AudioManager.Instance.PlayEffect(_timerClip);
    }

    private void HideInfoImages()
    {
        _timerImage.HideImmediate();
        _moneyImage.HideImmediate();
        _timeUnloadedImage.HideImmediate();
    }

    private IEnumerator SetClickable()
    {
        float clickDelay = 3f;
        yield return new WaitForSeconds(clickDelay);
        _isClickable = true;
    }
}