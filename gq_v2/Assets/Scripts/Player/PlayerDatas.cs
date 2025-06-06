using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDatas")]
public class PlayerDatas : ScriptableObject
{
    [Header("===== Move =====")]
    public float walkSpeed;
    public float runSpeed;
    [Header("===== Rotation =====")]
    public float rotationSpeed;
    [Header("===== Dash =====")]
    public float dashDelay;
    public float dashDuration;
    public float dashForce;
}
