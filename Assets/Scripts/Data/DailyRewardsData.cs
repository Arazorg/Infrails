using System;

[Serializable]
public class DailyRewardsData 
{
    public string NextDailyRewardTime;
    public string EndOfTempAmplification;
    public string NextAdsRewardTime;
    public int NumberOfMoneyRewards;
    public int NumberOfAmplificationRewards;
    public bool isTimeChanged;

    public DailyRewardsData()
    {
        NextDailyRewardTime = string.Empty;
        NextAdsRewardTime = string.Empty;
        EndOfTempAmplification = string.Empty;
        NumberOfMoneyRewards = 5;
        NumberOfAmplificationRewards = 10;
        isTimeChanged = false;
    }
}
