using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Element/Standart Element", fileName = "New Element")]
public class Element : ScriptableObject
{
    private const float StandartInteraction = 1;

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

    public int GetDamageWithResistance(int damage, Type type)
    {
        ElementResistance elementResistance = _elementsResistances.Where(s => s.ElementType == type).FirstOrDefault();
        if (elementResistance != null)
            return (int)(damage * elementResistance.Resistance);
        else
            return damage;
    }

    public float GetElementInteractionByType(Type type)
    {
        ElementResistance elementResistance = _elementsResistances.Where(s => s.ElementType == type).FirstOrDefault();
        if (elementResistance != null)
            return elementResistance.Resistance;
        else
            return StandartInteraction;
    }
}
