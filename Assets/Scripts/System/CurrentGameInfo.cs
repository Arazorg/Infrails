using UnityEngine;

public class CurrentGameInfo : MonoBehaviour
{
    public static CurrentGameInfo Instance;

    public bool IsExpert;
    public bool IsInfinite;
    public CharacterData CharacterData;
    public TrolleyData TrolleyData;
    public PassiveSkillData PassiveSkillData;
    public float GameStartTime;
    public int CountOfEarnedMoney;
    public int CountOfKilledEnemies;
    public int ReachedBiomeNumber;

    [SerializeField] private AnalyticsManager _analyticsManager;

    public delegate void ReachedLevel();
    public event ReachedLevel OnReachedLevel;

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
        IsInfinite = false;
        CharacterData = null;
        PassiveSkillData = null;
        TrolleyData = null;
        ReachedBiomeNumber = 1;
        CountOfKilledEnemies = 0;
        CountOfEarnedMoney = 0;
    }

    public void AddEnemyDeath()
    {
        CountOfEarnedMoney++;
        CountOfKilledEnemies++;
    }

    public void AddReachedBiome()
    {
        ReachedBiomeNumber++;
        OnReachedLevel?.Invoke();
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
