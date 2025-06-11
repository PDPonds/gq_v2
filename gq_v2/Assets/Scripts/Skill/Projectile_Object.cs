using UnityEngine;

public class Projectile_Object : MonoBehaviour
{
    float speed;
    Vector3 dir;
    Rigidbody rb;

    public void SetupProjectile(float speed, float duration, Vector3 dir)
    {
        rb = GetComponent<Rigidbody>();
        this.dir = dir;
        this.speed = speed;
        Destroy(gameObject, duration);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {

    }

}
