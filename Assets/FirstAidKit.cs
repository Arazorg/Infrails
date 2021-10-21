using UnityEngine;

public class FirstAidKit : Enemy
{
    private const int HealValue = 5;

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
        _character.Heal(HealValue);
        SpawnExplosionParticle();
        Destroy(gameObject);
    }

    private void GetCharacter(GameObject target)
    {
        EnemiesManager.Instance.OnTargetInit -= GetCharacter;
        _character = target.GetComponent<Character>();
    }
}
