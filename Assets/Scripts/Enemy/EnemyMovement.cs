using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform enemyShadowTransform;
#pragma warning restore 0649

    public Transform Target
    {
        set { target = value; }
    }
    public Transform target;

    public Transform SpawnPoint
    {
        set { spawnPoint = value; }
    }
    private Transform spawnPoint;

    public bool IsChase
    {
        set
        {
            isChase = value;
            if (isChase)
                StartCoroutine(GetNextPoint());
            else
                StopCoroutine(GetNextPoint());
        }
    }
    private bool isChase;

    public bool IsMove
    {
        set
        {
            isMove = value;
        }
    }
    private bool isMove = true;

    private Vector3 needPosition;
    private Quaternion needQuaternion;
    private Quaternion startShadowQuaternion;
    private bool isFacingRight;

    void Start()
    {
        startShadowQuaternion = enemyShadowTransform.rotation;
    }

    void FixedUpdate()
    {
        FlipToTarget();
        MoveToTarget();
        RotateToTarget();
        enemyShadowTransform.rotation = startShadowQuaternion;
    }

    private void MoveToTarget()
    {
        float minDistanceX = 10f;
        if (target != null && isChase)
        {
            if (System.Math.Abs(target.position.x - transform.position.x) < minDistanceX)
                GetNextPoint();
            SetStateOfAttack();
        }

        if (isMove)
            transform.position = Vector3.Lerp(transform.position, needPosition, Time.fixedDeltaTime / 0.33f);
        else
            GetComponent<EnemyAttack>().IsAttack = false;
    }

    public void MoveToSpawnPoint()
    {
        StopCoroutine(GetNextPoint());
        isChase = false;
        target = spawnPoint;
        needPosition = spawnPoint.position;
        Destroy(gameObject, 2f);
    }

    private void SetStateOfAttack()
    {
        float minDistance = 2f;
        float maxDistance = 7f;
        var distance = transform.position.y - target.position.y;
        if (minDistance < distance && distance < maxDistance)
            GetComponent<EnemyAttack>().IsAttack = true;
        else
            GetComponent<EnemyAttack>().IsAttack = false;
    }

    private IEnumerator GetNextPoint()
    {
        float minDistanceX = 15f;

        if (target != null)
        {
            while (true)
            {
                if (target.position.x > spawnPoint.position.x)
                {
                    needPosition.x = target.position.x + Random.Range(-10f, 0f) + -minDistanceX;
                    needPosition.y = target.position.y + Random.Range(-5f, 35f);
                }
                else if (target.position.x < spawnPoint.position.x)
                {
                    needPosition.x = target.position.x + Random.Range(0f, 10f) + minDistanceX;
                    needPosition.y = target.position.y + Random.Range(-5f, 35f);
                }
                yield return new WaitForSeconds(0.85f + Random.Range(-0.1f, 0.1f));

                if (Random.Range(0, 100) < 15f)
                    spawnPoint.position = new Vector3(spawnPoint.position.x * -1, spawnPoint.position.y, spawnPoint.position.z);
            }
        }
        yield return null;
    }

    private void RotateToTarget()
    {
        if (target != null)
        {
            if (transform.position.y < target.position.y)
                needQuaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(-15, -5)));
            else
                needQuaternion = Quaternion.Euler(new Vector3(0, 0, Random.Range(5, 15)));

            transform.rotation = Quaternion.Lerp(transform.rotation, needQuaternion, Time.deltaTime / 0.4f);
        }
    }

    private void FlipToTarget()
    {
        if (target != null)
        {
            if (target.position.x < transform.position.x && isFacingRight)
                Flip();
            else if (target.position.x > transform.position.x && !isFacingRight)
                Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
