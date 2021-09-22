using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Amplifications/Standart Amplification", fileName = "New Amplification")]

public class AmplificationData : ItemData
{
    [SerializeField] private AmplificationType _amplificationType;
    [SerializeField] private AmplificationIncreaseType _amplificationIncreaseType;
    [SerializeField] private List<int> _amplificationPowers;

    public enum AmplificationIncreaseType
    {
        Percent,
        Add
    }

    public enum AmplificationType
    {
        Health,
        Armor,
        Damage,
        Time,
        Resistance,
        Chance
    }

    public AmplificationType CurrentAmplificationType => _amplificationType;

    public AmplificationIncreaseType CurrentAmplificationIncreaseType => _amplificationIncreaseType;

    public List<int> AmplificationPowers => _amplificationPowers;

    public int Level { get; set; }
}
