using UnityEngine;

public class RepairKit : Enemy
{
    private const int RepairValue = 3;

    private Character _character;

    public override void Init(EnemyData data, Transform spawnPoint, GameObject target)
    {
        Data = data;
        OnInit();
        if (target == null)
            EnemiesManager.Instance.OnTargetInit += GetCharacter;
        else
            _character = target.GetComponent<Character>();
    }

    protected override void Death()
    {
        _character.HealArmor(RepairValue);
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void GetCharacter(GameObject target)
    {
        EnemiesManager.Instance.OnTargetInit -= GetCharacter;
        _character = target.GetComponent<Character>();
    }
}
