using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(NavMeshAgent))]
public class AIController : MonoBehaviour, ICombatable
{
    public AISO enemy;
    public aiBehavior behavior;

    public int curHp { get; set; }

    Rigidbody rb;
    CapsuleCollider col;
    NavMeshAgent agent;
    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        anim.runtimeAnimatorController = enemy.AIAnimator;
        curHp = enemy.maxHP;
    }

    private void Update()
    {
        UpdateBehavior();
    }

    public void Heal(int amount)
    {

    }

    public void TakeDamage(int damage, float knockbackForce, float knockbackDuration)
    {
        curHp -= damage;
        StartCoroutine(AddForce(-transform.forward, knockbackForce, knockbackDuration));
        SwitchBehavior(aiBehavior.TakeDamage);
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
            case aiBehavior.Runaway:
                break;
            case aiBehavior.Chase:
                break;
            case aiBehavior.ChangeAttack:
                break;
            case aiBehavior.Attack:
                break;
            case aiBehavior.TakeDamage:
                StartCoroutine(WaitForAnimationEnd("TakeDamage", Before_TakeDamage, After_TakeDamage));
                break;
            case aiBehavior.Death:
                break;
        }
    }

    void Before_TakeDamage()
    {
        if (enemy.type != AIType.Dummy)
        {
            if (curHp <= 0)
            {
                curHp = 0;
                Death();
            }
        }
    }

    void After_TakeDamage()
    {
        switch (enemy.type)
        {
            case AIType.Peaceful:
                SwitchBehavior(aiBehavior.Runaway);
                break;
            case AIType.Defensive:
                SwitchBehavior(aiBehavior.Chase);
                break;
            case AIType.Aggressive:
                SwitchBehavior(aiBehavior.Chase);
                break;
            case AIType.Dummy:
                SwitchBehavior(aiBehavior.Idle);
                break;
        }
    }

    void UpdateBehavior()
    {
        switch (this.behavior)
        {
            case aiBehavior.Idle:
                if (enemy.type == AIType.Dummy)
                {
                    LookAt(PlayerManager.Instance.transform.position);
                }
                agent.velocity = Vector3.zero;
                break;
            case aiBehavior.Runaway:
                break;
            case aiBehavior.Chase:

                break;
            case aiBehavior.ChangeAttack:
                agent.velocity = Vector3.zero;
                break;
            case aiBehavior.Attack:
                agent.velocity = Vector3.zero;
                break;
            case aiBehavior.TakeDamage:
                agent.velocity = Vector3.zero;
                break;
            case aiBehavior.Death:
                agent.velocity = Vector3.zero;
                break;
        }
    }

    public bool isBehavior(aiBehavior behavior)
    {
        return this.behavior == behavior;
    }

    IEnumerator WaitForAnimationEnd(string animationName, UnityAction beforeAnimation, UnityAction afterAnimation)
    {
        beforeAnimation?.Invoke();
        anim.Play(animationName);
        float duration = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);
        afterAnimation?.Invoke();
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
