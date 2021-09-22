using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Elements Resistance/Standart Elements Resistance", fileName = "New Elements Resistance")]
public class ElementsResistance : ScriptableObject
{
    [SerializeField] private Elements element;
    [SerializeField] private List<DamageMultiplier> damageMultipliers;

    public enum Elements
    {
        Default,
        Nature,
        Fire,
        Water,
        Earth,
        Void
    }

    public float GetDamageMultiplierByType(Elements currentElement)
    {
        foreach (var damageMultiplier in damageMultipliers)
        {
            if (damageMultiplier.Element == currentElement)
            {
                return damageMultiplier.Multiplier;
            } 
        }

        return 1;
    }

    [Serializable]
    public struct DamageMultiplier
    {
        public Elements Element;
        public float Multiplier;
    }
}
