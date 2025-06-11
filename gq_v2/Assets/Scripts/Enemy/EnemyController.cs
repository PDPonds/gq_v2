using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class EnemyController : MonoBehaviour, ICombatable
{
    public int curHp { get; set; }

    Rigidbody rb;
    CapsuleCollider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        LookAt(PlayerManager.Instance.transform.position);
    }

    public void Heal(int amount)
    {

    }

    public void TakeDamage(int damage, float knockbackForce, float knockbackDuration)
    {
        curHp -= damage;
        StartCoroutine(AddForce(-transform.forward, knockbackForce, knockbackDuration));

        //if (curHp <= 0)
        //{
        //    curHp = 0;
        //    Death();
        //}
    }

    public void Death()
    {
        Destroy(gameObject);
    }


    IEnumerator AddForce(Vector3 dir, float force, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            rb.AddForce(dir * force, ForceMode.Impulse);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.freezeRotation = true;
            rb.AddForce(dir * force, ForceMode.Impulse);
            yield return null;
        }

        yield return null;
        rb.linearVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        rb.freezeRotation = true;
    }

    void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

}
