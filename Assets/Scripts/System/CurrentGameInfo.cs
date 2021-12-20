using UnityEngine;

public class CurrentGameInfo : MonoBehaviour
{
    public static CurrentGameInfo Instance;

    public bool IsExpert;
    public CharacterData CharacterData;
    public TrolleyData TrolleyData;
    public PassiveSkillData PassiveSkillData;
    public float GameStartTime;
    public int CountOfEarnedMoney;
    public int CountOfKilledEnemies;
    public int ReachedBiomeNumber;

    [SerializeField] private AnalyticsManager _analyticsManager;

    public void AddResultsToProgress()
    {
        int numberMoneyForBiome = 5;
        CountOfEarnedMoney += ReachedBiomeNumber * numberMoneyForBiome;
        PlayerProgress.Instance.PlayerMoney += CountOfEarnedMoney;
        PlayerProgress.Instance.Save();
        _analyticsManager.OnPlayerDead(ReachedBiomeNumber, CharacterData.UnitName);
    }

    public void CreateNewGame()
    {
        IsExpert = false;
        CharacterData = null;
        PassiveSkillData = null;
        TrolleyData = null;
        ReachedBiomeNumber = 15;
        CountOfKilledEnemies = 0;
        CountOfEarnedMoney = 0;
    }

    public void AddEnemyDeath()
    {
        CountOfEarnedMoney++;
        CountOfKilledEnemies++;
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
            DontDestroyOnLoad(gameObject);
        }
    }
}
