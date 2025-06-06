using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerDatas data;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isRun;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Collider col;

    Vector3 moveDir;
    float curSpeed;
    bool isAddForceState;
    float curDashDelay;

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
    }

    #region Movement

    void MoveHandle()
    {
        if (!isAddForceState)
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
        if (isRun) curSpeed = data.runSpeed;
        else curSpeed = data.walkSpeed;
    }

    void RotationHandle()
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

    void MoveAnimationHandle()
    {
        if (moveInput != Vector2.zero)
        {
            if (isRun) anim.SetFloat("MoveSpeed", 1);
            else anim.SetFloat("MoveSpeed", 0.5f);
        }
        else anim.SetFloat("MoveSpeed", 0);
    }

    IEnumerator AddForce(Vector3 dir, float force, float duration)
    {
        if (!isAddForceState)
        {
            float startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                isAddForceState = true;
                anim.SetBool("isAddForceState", true);
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                rb.freezeRotation = true;
                rb.AddForce(dir * force, ForceMode.Impulse);
                yield return null;
            }
        }

        yield return null;
        anim.SetBool("isAddForceState", false);
        rb.constraints = RigidbodyConstraints.None;
        rb.freezeRotation = true;
        isAddForceState = false;
    }

    public void DashPerformed()
    {
        if (curDashDelay <= 0)
        {
            Vector3 dir = Vector3.zero;
            dir = Camera.main.transform.forward * moveInput.y;
            dir = dir + Camera.main.transform.right * moveInput.x;
            dir.Normalize();
            dir.y = 0;

            StartCoroutine(AddForce(dir, data.dashForce, data.dashDuration));
            curDashDelay = data.dashDelay;
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

}
