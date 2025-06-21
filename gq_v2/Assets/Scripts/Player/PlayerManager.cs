using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : Singleton<PlayerManager>
{
    public UnityAction OnDash;
    public UnityAction OnUseSkill_1;
    public UnityAction OnUseSkill_2;
    public UnityAction OnUseSkill_3;
    public UnityAction OnUseSkill_4;
    public UnityAction OnUseSkill_5;

    public PlayerDatas data;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector2 mousePosition;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Collider col;

    Vector3 moveDir;
    float curSpeed;
    bool isAction;
    float curDashDelay;

    float curSkill_1_Delay;
    float curSkill_2_Delay;
    float curSkill_3_Delay;
    float curSkill_4_Delay;
    float curSkill_5_Delay;

    Queue<UnityAction> bufferQueue = new Queue<UnityAction>();

    [Header("===== Init Skill Position =====")]
    [SerializeField] Transform Hand_Skill_Position;
    [SerializeField] Transform Body_Skill_Position;

    private void OnEnable()
    {
        OnDash += DashPerformed;
        OnUseSkill_1 += UseSkill_1;
        OnUseSkill_2 += UseSkill_2;
        OnUseSkill_3 += UseSkill_3;
        OnUseSkill_4 += UseSkill_4;
        OnUseSkill_5 += UseSkill_5;
    }

    private void OnDisable()
    {
        OnDash -= DashPerformed;
        OnUseSkill_1 -= UseSkill_1;
        OnUseSkill_2 -= UseSkill_2;
        OnUseSkill_3 -= UseSkill_3;
        OnUseSkill_4 -= UseSkill_4;
        OnUseSkill_5 -= UseSkill_5;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        SpeedHandle();
        MoveHandle();
        RotationHandle();
        MoveAnimationHandle();
        DelayDash();
        DelaySkill();
    }

    Vector3 GetMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 dir = (hitPoint - transform.position);
            dir.y = 0;
            return dir.normalized;
        }

        return Vector3.zero;
    }

    #region Movement

    void MoveHandle()
    {
        if (!isAction)
        {
            moveDir = Camera.main.transform.forward * moveInput.y;
            moveDir = moveDir + Camera.main.transform.right * moveInput.x;
            moveDir.Normalize();
            moveDir.y = 0;
            moveDir = moveDir * curSpeed;

            rb.linearVelocity = new Vector3(moveDir.x, rb.linearVelocity.y, moveDir.z);
        }
    }

    void SpeedHandle()
    {
        curSpeed = data.walkSpeed;
    }

    void RotationHandle()
    {
        if (!isAction)
        {
            Vector3 targetDir = Vector3.zero;
            targetDir = Camera.main.transform.forward * moveInput.y;
            targetDir = targetDir + Camera.main.transform.right * moveInput.x;
            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, data.rotationSpeed * Time.deltaTime);

                transform.rotation = playerRot;
            }
        }
    }

    void LookAt(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    void MoveAnimationHandle()
    {
        if (!isAction)
        {
            if (moveInput != Vector2.zero) anim.SetFloat("MoveSpeed", 1);
            else anim.SetFloat("MoveSpeed", 0);
        }
    }

    IEnumerator Dash(Vector3 dir, float force, float duration)
    {
        if (dir != Vector3.zero)
        {
            rb.linearVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            rb.freezeRotation = true;
            rb.linearVelocity = dir * force;
            isAction = true;
            anim.SetBool("isAddForceState", true);
            LookAt(transform.position + dir);

            yield return new WaitForSeconds(duration);

            rb.linearVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.None;
            rb.freezeRotation = true;
            anim.SetBool("isAddForceState", false);
            isAction = false;
            if (bufferQueue.Count > 0)
            {
                UnityAction action = bufferQueue.Dequeue();
                action?.Invoke();
            }
        }

    }

    void DashPerformed()
    {
        if (!isAction)
        {
            if (curDashDelay <= 0)
            {
                Vector3 dir = Vector3.zero;
                dir = Camera.main.transform.forward * moveInput.y;
                dir = dir + Camera.main.transform.right * moveInput.x;
                dir.Normalize();
                dir.y = 0;
                if (dir == Vector3.zero) dir = transform.forward;
                LookAt(transform.position + dir);
                StartCoroutine(Dash(dir, data.dashForce, data.dashDuration));
                curDashDelay = data.dashDelay;
            }
        }
        else
        {
            if (bufferQueue.Count <= 0)
                bufferQueue.Enqueue(DashPerformed);
            else
            {
                bufferQueue.Dequeue();
                bufferQueue.Enqueue(DashPerformed);
            }
        }
    }

    void DelayDash()
    {
        if (curDashDelay > 0)
        {
            curDashDelay -= Time.deltaTime;
            if (curDashDelay <= 0)
            {
                curDashDelay = 0;
            }
        }
    }

    #endregion

    #region Attack

    void UseSkill_1()
    {
        if (!isAction)
        {
            if (data.skill_1 != null && !isAction && curSkill_1_Delay == 0)
            {
                ActivateSkill(data.skill_1);
                curSkill_1_Delay = data.skill_1.skillDelay;
            }
        }
        else
        {
            if (bufferQueue.Count <= 0)
                bufferQueue.Enqueue(UseSkill_1);
            else
            {
                bufferQueue.Dequeue();
                bufferQueue.Enqueue(UseSkill_1);
            }
        }
    }

    void UseSkill_2()
    {
        if (!isAction)
        {
            if (data.skill_2 != null && !isAction && curSkill_2_Delay == 0)
            {
                ActivateSkill(data.skill_2);
                curSkill_2_Delay = data.skill_2.skillDelay;
            }
        }
        else
        {
            if (bufferQueue.Count <= 0)
                bufferQueue.Enqueue(UseSkill_2);
            else
            {
                bufferQueue.Dequeue();
                bufferQueue.Enqueue(UseSkill_2);
            }
        }
    }

    void UseSkill_3()
    {
        if (!isAction)
        {
            if (data.skill_3 != null && !isAction && curSkill_3_Delay == 0)
            {
                ActivateSkill(data.skill_3);
                curSkill_3_Delay = data.skill_3.skillDelay;
            }
        }
        else
        {
            if (bufferQueue.Count <= 0)
                bufferQueue.Enqueue(UseSkill_3);
            else
            {
                bufferQueue.Dequeue();
                bufferQueue.Enqueue(UseSkill_3);
            }
        }
    }

    void UseSkill_4()
    {
        if (!isAction)
        {
            if (data.skill_4 != null && !isAction && curSkill_4_Delay == 0)
            {
                ActivateSkill(data.skill_4);
                curSkill_4_Delay = data.skill_4.skillDelay;
            }
            else
            {
                if (bufferQueue.Count <= 0)
                    bufferQueue.Enqueue(UseSkill_4);
                else
                {
                    bufferQueue.Dequeue();
                    bufferQueue.Enqueue(UseSkill_4);
                }
            }
        }
    }

    void UseSkill_5()
    {
        if (!isAction)
        {
            if (data.skill_5 != null && curSkill_5_Delay == 0)
            {
                ActivateSkill(data.skill_5);
                curSkill_5_Delay = data.skill_5.skillDelay;
            }
        }
        else
        {
            if (bufferQueue.Count <= 0)
                bufferQueue.Enqueue(UseSkill_5);
            else
            {
                bufferQueue.Dequeue();
                bufferQueue.Enqueue(UseSkill_5);
            }
        }
    }

    void DelaySkill()
    {
        if (data.skill_1 != null)
        {
            if (curSkill_1_Delay > 0)
            {
                curSkill_1_Delay -= Time.deltaTime;
                if (curSkill_1_Delay <= 0)
                {
                    curSkill_1_Delay = 0;
                }
            }
        }
        if (data.skill_2 != null)
        {
            if (curSkill_2_Delay > 0)
            {
                curSkill_2_Delay -= Time.deltaTime;
                if (curSkill_2_Delay <= 0)
                {
                    curSkill_2_Delay = 0;
                }
            }
        }
        if (data.skill_3 != null)
        {
            if (curSkill_3_Delay > 0)
            {
                curSkill_3_Delay -= Time.deltaTime;
                if (curSkill_3_Delay <= 0)
                {
                    curSkill_3_Delay = 0;
                }
            }
        }
        if (data.skill_4 != null)
        {
            if (curSkill_4_Delay > 0)
            {
                curSkill_4_Delay -= Time.deltaTime;
                if (curSkill_4_Delay <= 0)
                {
                    curSkill_4_Delay = 0;
                }
            }
        }
        if (data.skill_5 != null)
        {
            if (curSkill_5_Delay > 0)
            {
                curSkill_5_Delay -= Time.deltaTime;
                if (curSkill_5_Delay <= 0)
                {
                    curSkill_5_Delay = 0;
                }
            }
        }
    }

    IEnumerator AddForceAttack(Vector3 dir, float force, float duration)
    {
        rb.linearVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.freezeRotation = true;
        rb.AddForce(dir * force, ForceMode.Impulse);
        LookAt(transform.position + dir);

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        rb.freezeRotation = true;
    }

    IEnumerator StopMoveAttack(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            isAction = true;
            yield return null;
        }
        yield return null;
        isAction = false;
        if (bufferQueue.Count > 0)
        {
            UnityAction action = bufferQueue.Dequeue();
            action?.Invoke();
        }
    }

    IEnumerator InitParticle(SkillSO skill, GameObject prefab, Transform spawnPos, float duration, Vector3 dir, float knockbackForce, float knockbackDuration)
    {
        yield return new WaitForSeconds(duration);
        InitParticle(skill, prefab, spawnPos, dir, knockbackForce, knockbackDuration);
    }

    void InitParticle(SkillSO skill, GameObject prefab, Transform spawnPos, Vector3 dir, float knockbackForce, float knockbackDuration)
    {
        GameObject particleObj = Instantiate(prefab, spawnPos.position, Quaternion.identity);
        if (skill is Skill_Projectile projectile)
        {
            Projectile_Object projectile_Object = particleObj.GetComponent<Projectile_Object>();
            projectile_Object.SetupProjectile(skill.skillDamage, projectile.projectileSpeed, projectile.projectileDuration, dir, knockbackForce, knockbackDuration);
        }
        else if (skill is Skill_BuffPlayer buffPlayer)
        {

        }
        else if (skill is Skill_AOE_AroundMask aroundMask)
        {

        }
        else if (skill is Skill_AOE_AroundPlayer aroundPlayer)
        {

        }
    }

    void ActivateSkill(SkillSO skill)
    {
        float startTime = Time.time;

        Skill_Visual_Property property = skill.skill_Property[skill.activateCount];
        GameObject particlePrefab = property.particle;
        AnimatorOverrideController animationController = property.animationController;
        InitSkillPosition intiPosition = property.intiPosition;
        Transform spawnPosition = GetSkillPosition(intiPosition);

        Vector3 dir = GetMouseDirection();
        if (dir == Vector3.zero) dir = transform.forward;

        anim.runtimeAnimatorController = animationController;
        anim.Play("Skill");
        float duration = property.skillForceDuration;
        float skillForce = property.skillForce;

        StartCoroutine(StopMoveAttack(property.skillDuration));
        StartCoroutine(AddForceAttack(dir, skillForce, duration));
        StartCoroutine(InitParticle(skill, particlePrefab, spawnPosition, property.initPrefabTime, dir, property.knockbackForce, property.knockbackDuration));

        int propertyCount = skill.skill_Property.Count;
        if (skill.activateCount >= propertyCount - 1) skill.activateCount = 0;
        else skill.activateCount++;

    }

    public Transform GetSkillPosition(InitSkillPosition posType)
    {
        Transform pos = transform;
        switch (posType)
        {
            case InitSkillPosition.Hand:
                pos = Hand_Skill_Position;
                break;
            case InitSkillPosition.Body:
                pos = Body_Skill_Position;
                break;
        }
        return pos;
    }

    #endregion
}
