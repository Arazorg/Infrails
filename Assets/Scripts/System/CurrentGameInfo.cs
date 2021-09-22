using UnityEngine;

public class CurrentGameInfo : MonoBehaviour
{
    public static CurrentGameInfo Instance;

    public bool IsExpert;
    public CharacterData CharacterData;
    public float GameStartTime;
    public int CountOfEarnedMoney;
    public int CountOfKilledEnemies;
    public int ReachedBiomeNumber;

    public void AddResultsToProgress()
    {
        PlayerProgress.Instance.PlayerMoney += CountOfEarnedMoney;
        PlayerProgress.Instance.Save();
    }

    public void CreateNewGame()
    {
        IsExpert = false;
        CharacterData = null;
        ReachedBiomeNumber = 1;
        CountOfKilledEnemies = 0;
        CountOfEarnedMoney = 0;
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
