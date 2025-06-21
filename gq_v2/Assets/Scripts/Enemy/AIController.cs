using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
public class AIController : MonoBehaviour, ICombatable
{
    public AISO enemy;
    [HideInInspector] public aiBehavior behavior;

    public int curHp { get; set; }

    Rigidbody rb;
    CapsuleCollider col;
    NavMeshAgent agent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateBehavior();
        LookAt(PlayerManager.Instance.transform.position);
    }

    public void Heal(int amount)
    {

    }

    public void TakeDamage(int damage, float knockbackForce, float knockbackDuration)
    {
        curHp -= damage;
        StartCoroutine(AddForce(-transform.forward, knockbackForce, knockbackDuration));

        if (curHp <= 0)
        {
            curHp = 0;
            Death();
        }
    }

    public void Death()
    {
        SwitchBehavior(aiBehavior.Death);
    }

    #region Behavior

    public void SwitchBehavior(aiBehavior behavior)
    {
        this.behavior = behavior;
        switch (this.behavior)
        {
            case aiBehavior.Idle:
                break;
            case aiBehavior.Chase:
                break;
            case aiBehavior.ChangeAttack:
                break;
            case aiBehavior.Attack:
                break;
            case aiBehavior.Death:
                break;
        }
    }

    void UpdateBehavior()
    {
        switch (this.behavior)
        {
            case aiBehavior.Idle:
                agent.velocity = Vector3.zero;
                break;
            case aiBehavior.Chase:

                break;
            case aiBehavior.ChangeAttack:
                agent.velocity = Vector3.zero;
                break;
            case aiBehavior.Attack:
                break;
            case aiBehavior.Death:
                break;
        }
    }

    public bool isBehavior(aiBehavior behavior)
    {
        return this.behavior == behavior;
    }

    #endregion

    IEnumerator AddForce(Vector3 dir, float force, float duration)
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir * force, ForceMode.Impulse);
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
        rb.AddForce(dir * force, ForceMode.Impulse);

        yield return new WaitForSeconds(duration);
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
