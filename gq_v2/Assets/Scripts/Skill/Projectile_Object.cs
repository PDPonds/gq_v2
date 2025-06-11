using UnityEngine;

public class Projectile_Object : MonoBehaviour
{
    float speed;
    Vector3 dir;
    Rigidbody rb;
    int damage;
    float knockbackForce;
    float knockbackDuration;

    public void SetupProjectile(int damage, float speed, float duration, Vector3 dir, float knockbackForce, float knockbackDuration)
    {
        rb = GetComponent<Rigidbody>();
        this.damage = damage;
        this.dir = dir;
        this.speed = speed;
        this.knockbackForce = knockbackForce;
        this.knockbackDuration = knockbackDuration;
        Destroy(gameObject, duration);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<ICombatable>(out ICombatable icombatable))
            {
                Debug.Log("TakeDamage");
                icombatable.TakeDamage(damage, knockbackForce, knockbackDuration);
            }
        }
    }

}
