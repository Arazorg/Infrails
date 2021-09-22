using UnityEngine;

[CreateAssetMenu(menuName = "MoneyRewards/Standart Money Rewards", fileName = "New Money Reward")]
public class MoneyReward : ItemData
{
    [SerializeField] private int _money;

    public int Money => _money;
}
