using UnityEngine;

public class Egg : Enemy
{
    private const float WiggleDuration = 0.33f;

    private Transform shadowTransform;
    private float wiggleTime = 0;
    private float startAngle;
    private float finishAngle;

    public override void Death()
    {
        EnemySpawner.Instance.SpawnEnemiesFromEgg(transform.parent);
        Destroy(gameObject);
    }

    private void Start()
    {
        shadowTransform = transform.GetChild(0);
        GetSideOfWiggle();
    }

    private void GetSideOfWiggle()
    {
        startAngle = 0;
        if (Random.Range(0, 2) % 2 == 0)
        {
            finishAngle = 10;
        }           
        else
        {
            finishAngle = -10;
        }     
    }

    private void FixedUpdate()
    {
        EggWiggle();
    }

    private void EggWiggle()
    {
        if (wiggleTime < WiggleDuration)
        {
            float z = Mathf.Lerp(startAngle, finishAngle, wiggleTime / WiggleDuration);
            transform.rotation = Quaternion.Euler(0, 0, z);
            shadowTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -transform.rotation.z));
            wiggleTime += Time.fixedDeltaTime;
        }
        else
        {
            wiggleTime = 0;
            startAngle = finishAngle;
            finishAngle *= -1;
        }
    }
}
