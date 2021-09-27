using System;
using System.Collections;
using UnityEngine;

public class DailyRewardsManager : MonoBehaviour
{
    public static DailyRewardsManager Instance;

    private const string DailyRewardsFile = "DailyRewards.txt";
    private const int MaxNumberOfMoneyRewards = 5;
    private const int MaxNumberOfAmplificationsRewards = 10;

    private DailyRewardsData _dailyRewardsData;
    private SaveSystem _saveSystem;
    private TimeSpan timeToAdsRewards;
    private TimeSpan timeToDailyReward;

    public int NumberOfMoneyRewards
    {
        get
        {
            return _dailyRewardsData.NumberOfMoneyRewards;
        }

        set
        {
            if (_dailyRewardsData.NumberOfMoneyRewards - 1 >= 0)
            {
                _dailyRewardsData.NumberOfMoneyRewards = value;
                Save();
            }
        }
    }

    public int NumberOfAmplificationRewards
    {
        get
        {
            return _dailyRewardsData.NumberOfAmplificationRewards;
        }

        set
        {
            if (_dailyRewardsData.NumberOfAmplificationRewards - 1 >= 0)
            {
                _dailyRewardsData.NumberOfAmplificationRewards = value;
                Save();
            }
        }
    }

    public bool IsTimeChanged
    {
        get
        {
            return _dailyRewardsData.isTimeChanged;
        }

        set
        {
            _dailyRewardsData.isTimeChanged = value;
            Save();
        }
    }

    public event DailyRewardAvailable OnDailyRewardAvailable;

    public delegate void DailyRewardAvailable();

    public DateTime GetDailyRewardSavedTime()
    {
        if (_dailyRewardsData.NextDailyRewardTime != string.Empty)
        {
            return Convert.ToDateTime(_dailyRewardsData.NextDailyRewardTime);
        }
        else
        {
            return DateTime.MinValue;
        }
    }

    public void SetDailyRewardTime(string nextTime)
    {
        _dailyRewardsData.NextDailyRewardTime = nextTime;
        Save();
    }

    public DateTime GetAdsRewardsSavedTime()
    {
        if (_dailyRewardsData.NextAdsRewardTime != string.Empty)
        {
            return Convert.ToDateTime(_dailyRewardsData.NextAdsRewardTime);
        }
        else
        {
            return DateTime.MinValue;
        }
    }

    public void RefreshAdsRewards()
    {
        DateTime nextRewardTime = DateTime.Now + timeToAdsRewards;
        _dailyRewardsData.NextAdsRewardTime = nextRewardTime.ToString();
        _dailyRewardsData.NumberOfMoneyRewards = MaxNumberOfMoneyRewards;
        _dailyRewardsData.NumberOfAmplificationRewards = MaxNumberOfAmplificationsRewards;
        Save();
    }

    public void ResetAllRewardsTime()
    {
        DateTime nextRewardTime = DateTime.Now + timeToAdsRewards;
        _dailyRewardsData.NextAdsRewardTime = nextRewardTime.ToString();
        SetDailyRewardTime((DateTime.Now + timeToDailyReward).ToString());
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _saveSystem = new SaveSystem();
    }

    private void Start()
    {
        timeToAdsRewards = new TimeSpan(GameConstants.DayToAdsRewards, 0, 0, 0);
        timeToDailyReward = new TimeSpan(0, GameConstants.HoursToDailyReward, 0, 0);
        Load();
        StartCoroutine(CheckingRewardsAvailability());
    }

    private void Load()
    {
        _dailyRewardsData = new DailyRewardsData();
        string currentInfoDailyRewards = _saveSystem.Load(DailyRewardsFile);
        if (currentInfoDailyRewards != string.Empty)
        {
            var dailyRewardsData = JsonUtility.FromJson<DailyRewardsData>(currentInfoDailyRewards);
            _dailyRewardsData.NextDailyRewardTime = dailyRewardsData.NextDailyRewardTime;
            _dailyRewardsData.NextAdsRewardTime = dailyRewardsData.NextAdsRewardTime;
            _dailyRewardsData.EndOfTempAmplification = dailyRewardsData.EndOfTempAmplification;
            _dailyRewardsData.NumberOfMoneyRewards = dailyRewardsData.NumberOfMoneyRewards;
            _dailyRewardsData.NumberOfAmplificationRewards = dailyRewardsData.NumberOfAmplificationRewards;
        }

        Save();
        CheckAdsRewardsAvailability();
    }

    private void Save()
    {
        string currentInfoDailyRewards = JsonUtility.ToJson(_dailyRewardsData);
        _saveSystem.Save(currentInfoDailyRewards, DailyRewardsFile);
    }

    private void CheckAdsRewardsAvailability()
    {
        if (GetAdsRewardsSavedTime() <= DateTime.Now)
        {
            RefreshAdsRewards();
        }
        else if (GetAdsRewardsSavedTime().Ticks > timeToAdsRewards.Ticks)
        {
            if (DateTime.Now < GetAdsRewardsSavedTime() - timeToAdsRewards)
                IsTimeChanged = true;
        }
    }

    private IEnumerator CheckingRewardsAvailability()
    {
        while (true)
        {
            float delay = 10f;
            yield return new WaitForSeconds(delay);

            if (GetDailyRewardSavedTime() <= DateTime.Now)
            {
                OnDailyRewardAvailable?.Invoke();
            }

            if (GetAdsRewardsSavedTime() <= DateTime.Now)
            {
                CheckAdsRewardsAvailability();
            }
        }
    }
}
