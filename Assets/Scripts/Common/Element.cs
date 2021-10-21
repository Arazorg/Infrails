﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Element/Standart Element", fileName = "New Element")]
public class Element : ScriptableObject
{
    [SerializeField] private Type _type;
    [SerializeField] private List<ElementResistance> _elementsResistances;
    [SerializeField] private Sprite _elementSpriteUI;

    public Type ElementType => _type;

    public Sprite ElementSpriteUI => _elementSpriteUI;

    public enum Type
    {
        None,
        Nature,
        Earth,
        Fire,
        Water,
        Void
    }

    public float GetElementInteractionByType(Type elementType)
    {
        float standartInteraction = 1;
        ElementResistance elementResistance = _elementsResistances.Where(s => s.ElementType == elementType).FirstOrDefault();
        if (elementResistance != null)
            return elementResistance.Resistance;
        else
            return standartInteraction;
    }
}
