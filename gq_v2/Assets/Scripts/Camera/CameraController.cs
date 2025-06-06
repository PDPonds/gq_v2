using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    [Header("===== Follow =====")]
    [SerializeField] float smoothSpeed = 0.125f;

    float curYRotation = 0f;
    float moveRotationSpeed = 1f;

    private void Start()
    {
        SetupTarget(PlayerManager.Instance.transform);
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    public void SetupTarget(Transform target)
    {
        this.target = target;
    }

    public void SetYRotation(float yRotation, float speed)
    {
        curYRotation = yRotation;
        moveRotationSpeed = speed;
    }

    void FollowTarget()
    {
        if (target != null)
        {
            Vector3 desiredPostion = target.position;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
            transform.position = smoothedPos;
        }

        if (transform.rotation.y != curYRotation)
        {
            Quaternion quaternion = Quaternion.Euler(new Vector3(0, curYRotation, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, moveRotationSpeed * Time.deltaTime);
        }

    }
}
