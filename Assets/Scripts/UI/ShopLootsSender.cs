using System.Collections.Generic;
using UnityEngine;

public class ShopLootsSender : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<MoneyReward> _adMoneyRewards;
    [SerializeField] private ItemData _trolleyForSupportData;

    public ItemData GetLootData(LootboxData data)
    {
        var loots = PlayerProgress.Instance.GetLootsAvailableInShop(data);
        switch (data.TypeOfLootbox)
        {
            case LootboxData.Type.Weapon:
                var weaponData = loots[Random.Range(0, loots.Count)];
                PlayerProgress.Instance.SetWeaponAvailable(weaponData.ItemName);
                return weaponData;
            case LootboxData.Type.Skill:
                var skillData = loots[Random.Range(0, loots.Count)];
                PlayerProgress.Instance.SetPassiveSkillAvailable((skillData as PassiveSkillData).OwnerData.UnitName, skillData.ItemName);
                return skillData;
            case LootboxData.Type.Amplification:
                var amplificationData = loots[Random.Range(0, loots.Count)];
                PlayerProgress.Instance.IncrementAmplificationLevel(amplificationData.ItemName);
                return amplificationData;
            case LootboxData.Type.Money:
                return GetMoneyReward();
            case LootboxData.Type.Support:
                return _trolleyForSupportData;
        }

        return null;
    }

    public ItemData GetMoneyReward()
    {
        int number = DailyRewardsManager.Instance.NumberOfMoneyRewards;
        PlayerProgress.Instance.PlayerMoney += _adMoneyRewards[number - 1].Money;
        DailyRewardsManager.Instance.NumberOfMoneyRewards -= 1;
        return _adMoneyRewards[number - 1];
    }
}
