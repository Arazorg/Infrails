using UnityEngine;

public class CharacterSkill : MonoBehaviour
{
    private Character character;

    public void UseSkill()
    {
        
    }

    private void Start()
    {
        character = GetComponent<Character>();
    }
}
