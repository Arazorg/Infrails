using System.Collections.Generic;
using UnityEngine;

public class CharacterAmplifications : MonoBehaviour
{
    private List<AmplificationData> _currentAmplificationsData;

    public List<AmplificationData> CurrentAmplificationsData => _currentAmplificationsData;

    public void Init()
    {
        int maxNumberAmplifications = 5;
        _currentAmplificationsData = new List<AmplificationData>(maxNumberAmplifications);
    }

    public void SetNewAmplification(AmplificationData amplificationData)
    {

    }
}
