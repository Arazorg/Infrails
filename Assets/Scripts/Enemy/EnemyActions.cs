using UnityEngine;

public class EnemyActions : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject popUpTextHandlerPrefab;
#pragma warning restore 0649

    public void CreateDeathEffect(Color color)
    {
        if (GetComponentInChildren<PopUpText>() != null)
            GetComponentInChildren<PopUpText>().transform.parent = null;
        GetComponent<BoxCollider2D>().enabled = false;
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        var settings = explosion.GetComponent<ParticleSystem>().main;
        settings.startColor = new ParticleSystem.MinMaxGradient(color);
        Destroy(explosion, settings.startLifetimeMultiplier);
    }
}
