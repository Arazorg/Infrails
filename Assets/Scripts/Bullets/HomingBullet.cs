using UnityEngine;

public class HomingBullet : Bullet, IUpdateable
{
    public Transform target;

    public float angleChangingSpeed = 5f;

    private void Start()
    {
        UpdateManager.Instance.Register(this);
    }

    public void Tick()
    {
        //Vector2 direction = (Vector2)target.position - Rigidbody.position;
       // direction.Normalize();
        //float rotateAmount = Vector3.Cross(direction, transform.up).z;
        //Rigidbody.angularVelocity = -angleChangingSpeed * rotateAmount;
       // Rigidbody.velocity = transform.up * BulletSpeed;
    }

    public override void BulletHit(Transform target)
    {
        HideBullet();
    }
}
