using UnityEngine;

public class Chest : Enemy
{
    private const string FirstAidKit = "FirstAidKit";
    private const string RepairKit = "RepairKit";

    public override void Death()
    {
        switch (data.UnitName)
        {
            case FirstAidKit:
                UseFirstAidKit();
                break;
            case RepairKit:
                UseRepairKit();
                break;
        }

        isDeath = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<EnemyActions>().CreateDeathEffect(data.UnitColor);
        Destroy(gameObject);
    }

    private void UseFirstAidKit()
    {
        characterScript.Heal(5);
    }

    private void UseRepairKit()
    {
        characterScript.HealArmor(3);
    }
}
