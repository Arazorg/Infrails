using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

public class GameShopLootsSender : MonoBehaviour
{
    private List<AmplificationData> _currentAmplifications = new List<AmplificationData>();

    public void Init(CharacterAmplifications characterAmplifications)
    {
        characterAmplifications.OnChangeAmplifications += SetCurrentAmplifications;
    }

    public ItemData GetLootData(GameShopProductData data)
    {
        int biomeForLootLevel = 15;
        int lootLevel = CurrentGameInfo.Instance.ReachedBiomeNumber / biomeForLootLevel;

        switch (data.ProductType)
        {
            case GameShopProductData.Type.AmplificationLootbox:
                int amplificationLevel = lootLevel + 1;
                return GetAmplificationData(amplificationLevel);
            case GameShopProductData.Type.WeaponLootbox:
                return GetWeaponData(lootLevel);
        }

        return null;
    }

    private void SetCurrentAmplifications(List<AmplificationData> currentAmplifications)
    {
        _currentAmplifications = currentAmplifications;
    }

    private WeaponData GetWeaponData(int weaponLevel)
    {
        bool isLevelReduced = false;
        int maxNumberStars = 5;
        var availableWeapons = GetAvailableWeapons(weaponLevel);
        while (availableWeapons.Count == 0)
        {
            weaponLevel--;
            availableWeapons = GetAvailableWeapons(weaponLevel);
            isLevelReduced = true;
        }

        var weapon = availableWeapons.OrderBy(qu => Guid.NewGuid()).First();
        if (!isLevelReduced)
            weapon.StarsNumber = GetWeaponStarsNumber();
        else
            weapon.StarsNumber = maxNumberStars;

        return weapon;
    }

    private AmplificationData GetAmplificationData(int amplificationLevel)
    {
        var availableAmplifications = GetAvailableAmplifications(amplificationLevel);

        while (availableAmplifications.Count == 0)
        {
            amplificationLevel--;
            availableAmplifications = GetAvailableAmplifications(amplificationLevel);
        }

        var amplificationData = availableAmplifications.OrderBy(qu => Guid.NewGuid()).First();
        amplificationData.Level = amplificationLevel;
        return amplificationData;
    }

    private List<AmplificationData> GetAvailableAmplifications(int amplificationLevel)
    {
        var allAmplifications = PlayerProgress.Instance.GetAmplificationsData(true);
        var amplifications = allAmplifications.Where(s => s.Level == amplificationLevel).ToList();
        foreach (var amplificationData in _currentAmplifications)
            amplifications.Remove(amplificationData);

        return amplifications;
    }

    private List<WeaponData> GetAvailableWeapons(int weaponLevel)
    {
        var allWeapons = PlayerProgress.Instance.GetWeaponsData(true);
        var weapons = allWeapons.Where(s => s.Level == weaponLevel).ToList();
        return weapons;
    }

    private int GetWeaponStarsNumber()
    {
        float biomeForLootLevel = 15;
        int biomeForStar = 3;
        float tempStarsNumber = (CurrentGameInfo.Instance.ReachedBiomeNumber % biomeForLootLevel) / biomeForStar;
        if ((tempStarsNumber - (int)tempStarsNumber) >= 0)
            tempStarsNumber++;

        if (UnityEngine.Random.value < 0.5f)
            tempStarsNumber++;

        if (tempStarsNumber > 5)
            tempStarsNumber = 5;

        return (int)tempStarsNumber;
    }
}
