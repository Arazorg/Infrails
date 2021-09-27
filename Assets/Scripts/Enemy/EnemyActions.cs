using UnityEngine;

public class EnemyActions : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    public void CreateDeathEffect(Color color)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        var settings = explosion.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);
        Destroy(explosion, settings.startLifetimeMultiplier);
    }
}
