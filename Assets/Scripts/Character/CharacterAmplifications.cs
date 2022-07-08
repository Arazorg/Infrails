using System.Collections.Generic;
using UnityEngine;

public class CharacterAmplifications : MonoBehaviour
{
    private const int MaxNumberAmplifications = 3;

    private List<AmplificationData> _currentAmplificationsData;

    public delegate void ChangeAmplifications(List<AmplificationData> currentAmplificationsData);

    public event ChangeAmplifications OnChangeAmplifications;

    public List<AmplificationData> CurrentAmplificationsData
    {
        set { _currentAmplificationsData = value; }
    }

    public void Init()
    {
        _currentAmplificationsData = new List<AmplificationData>(MaxNumberAmplifications);
    }

    public bool AddNewAmplification(AmplificationData amplificationData)
    {
        if (_currentAmplificationsData.Count < MaxNumberAmplifications)
        {
            _currentAmplificationsData.Add(amplificationData);
            OnChangeAmplifications?.Invoke(_currentAmplificationsData);
            return true;
        }

        return false;
    }

    public void DeleteAmplification(AmplificationData amplificationData)
    {
        _currentAmplificationsData.Remove(amplificationData);
        for (int i = 0; i < _currentAmplificationsData.Capacity; i++)
        {
            if (i < _currentAmplificationsData.Count - 1)
            {
                if (_currentAmplificationsData[i] == null)
                {
                    _currentAmplificationsData[i] = _currentAmplificationsData[i + 1];
                    _currentAmplificationsData[i + 1] = null;
                }
            }
        }

        OnChangeAmplifications?.Invoke(_currentAmplificationsData);
    }
}