using UnityEngine;

public class ManeCrystal : Enemy
{
    private const float wiggleDuration = 0.5f;

    private Transform shadowTransform;
    private Vector3 crystalShadowOffset = new Vector3(0, -2, 0);
    private Vector3 startPosition;
    private Vector3 startOffset;
    private Vector3 finishOffset;
    private float wiggleTime = 0;

    private void Start()
    {
        shadowTransform = transform.GetChild(0);
        startPosition = transform.position;
        SetStartOffset();
    }

    private void SetStartOffset()
    {       
        if (Random.Range(0, 10) % 2 == 0)
        {
            startOffset = new Vector3(0, 1f, 0);
            finishOffset = new Vector3(0, -1f, 0);
        }            
        else
        {
            startOffset = new Vector3(0, -1f, 0);
            finishOffset = new Vector3(0, 1f, 0);
        }
        transform.position += startOffset;
    }

    private void FixedUpdate()
    {
        ManeCrystalWiggle();
    }

    private void ManeCrystalWiggle()
    {
        if (wiggleTime < wiggleDuration)
        {
            Vector3 currentPosition = Vector3.Lerp(startPosition + startOffset, startPosition + finishOffset, wiggleTime / wiggleDuration);
            transform.position = currentPosition;
            shadowTransform.position = transform.parent.position + crystalShadowOffset;
            wiggleTime += Time.fixedDeltaTime;
        }
        else
        {
            wiggleTime = 0;
            startOffset = finishOffset;
            finishOffset *= -1;
        }
    }

    public override void Death()
    {
        
        Destroy(gameObject);
    }
}
