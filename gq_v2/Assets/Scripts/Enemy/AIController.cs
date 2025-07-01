using System.Collections;
using UnityEditor.Experimental.GraphView;
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

    float curChangeDuration;

    float attackDelay;
    AISkill currentSkill;

    [SerializeField] GameObject change_Projectile;
    [SerializeField] GameObject change_AOE_AroundUser;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        anim.runtimeAnimatorController = enemy.AIAnimator;
        curHp = enemy.maxHP;

        currentSkill = enemy.RandomSkill();

    }

    private void Update()
    {
        UpdateBehavior();
        DelayAttack();
    }

    public void Heal(int amount)
    {

    }

    public void TakeDamage(int damage, float knockbackForce, float knockbackDuration)
    {
        curHp -= damage;
        if (!isBehavior(aiBehavior.ChangeAttack) && !isBehavior(aiBehavior.Attack))
        {
            StartCoroutine(AddForce(-transform.forward, knockbackForce, knockbackDuration));
            SwitchBehavior(aiBehavior.TakeDamage);
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
                agent.speed = enemy.walkSpeed;
                break;
            case aiBehavior.Runaway:
                agent.speed = enemy.runSpeed;
                break;
            case aiBehavior.Chase:
                agent.speed = enemy.runSpeed;
                break;
            case aiBehavior.ChangeAttack:
                curChangeDuration = currentSkill.changeDuration;
                InitChangePrefab(currentSkill);
                break;
            case aiBehavior.Attack:
                StartCoroutine(ActiveSkill(currentSkill));
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
                agent.velocity = Vector3.zero;
                break;
            case aiBehavior.Chase:

                float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
                if (distance <= currentSkill.skillRange && attackDelay == 0)
                {
                    SwitchBehavior(aiBehavior.ChangeAttack);
                }
                else
                {
                    agent.SetDestination(PlayerManager.Instance.transform.position);
                }

                break;
            case aiBehavior.ChangeAttack:
                agent.velocity = Vector3.zero;
                curChangeDuration -= Time.deltaTime;
                if (curChangeDuration <= 0)
                {
                    SwitchBehavior(aiBehavior.Attack);
                }
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

    void DelayAttack()
    {
        if (attackDelay > 0)
        {
            attackDelay -= Time.deltaTime;
            if (attackDelay < 0)
            {
                attackDelay = 0;
            }
        }
    }

    IEnumerator ActiveSkill(AISkill skill)
    {
        InitSkillParticle(currentSkill);
        yield return new WaitForSeconds(currentSkill.attackDuration);
        currentSkill = enemy.RandomSkill();
        attackDelay = skill.skillDelay;
        SwitchBehavior(aiBehavior.Chase);
    }

    void InitChangePrefab(AISkill skill)
    {
        if (skill is AISkill_Projectile projectile)
        {
            change_Projectile.SetActive(true);
            ProjectileChange change = change_Projectile.GetComponent<ProjectileChange>();
            change.Setup(projectile.projectile_Speed, projectile.projectile_Duration, skill.changeDuration, skill.changeSize);
        }
        if (skill is AISkill_AOE_AroundUser AOE_User)
        {
            change_AOE_AroundUser.SetActive(true);
            AreaChange change = change_AOE_AroundUser.GetComponent<AreaChange>();
            change.Setup(skill.changeDuration, skill.changeSize);
        }
    }

    void InitChangeParticle(AISkill skill)
    {
        Vector3 initPosition = transform.TransformPoint(skill.changeParticleOffset);
        GameObject particleObj = Instantiate(skill.changeParticle, initPosition, Quaternion.identity);
    }

    void InitSkillParticle(AISkill skill)
    {
        Vector3 initPosition = transform.TransformPoint(skill.attackParticleOffset);
        if (skill is AISkill_Projectile projectile)
        {
            GameObject particleObj = Instantiate(skill.attackParticle, initPosition, Quaternion.identity);
            Projectile_Object projectile_Object = particleObj.GetComponent<Projectile_Object>();
            projectile_Object.SetupProjectile(skill.skillDamage, projectile.projectile_Speed, projectile.projectile_Duration, transform.forward, 0, 0);

        }
        if (skill is AISkill_AOE_AroundUser aroundUser)
        {

        }
    }

}
