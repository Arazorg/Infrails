using System.Collections.Generic;
using UnityEngine;

public class CharacterElements : MonoBehaviour
{
    private CharacterSkill characterSkill;
    private Character character;
    private Weapon currentWeapon;
    private List<EnemyData> currentManeCrystals = new List<EnemyData>();

    private void Start()
    {
        character = GetComponent<Character>();
        characterSkill = GetComponent<CharacterSkill>();
        currentWeapon = GetComponentInChildren<Weapon>();
    }
}
