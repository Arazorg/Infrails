using UnityEngine;

public class Barrel : Enemy
{
    public override void Death()
    {
        isDeath = true;
        Explosion();
        Destroy(gameObject, 0.33f);
    }

    private void Explosion()
    {
        string explosionAnimationKey = "Explosion";
        GetComponent<Animator>().Play(explosionAnimationKey);
    }
}
