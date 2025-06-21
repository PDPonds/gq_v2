using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDatas")]
public class PlayerDatas : ScriptableObject
{
    [Header("===== Move =====")]
    public float walkSpeed;
    [Header("===== Rotation =====")]
    public float rotationSpeed;
    [Header("===== Action Buffer =====")]
    public float buffetDuration;
    [Header("===== Dash =====")]
    public float dashDelay;
    public float dashDuration;
    public float dashForce;
    [Header("===== Attack =====")]
    public SkillSO skill_1;
    public SkillSO skill_2;
    public SkillSO skill_3;
    public SkillSO skill_4;
    public SkillSO skill_5;
    [Header("===== AimAssist =====")]
    public LayerMask aimAssisMark;
}
