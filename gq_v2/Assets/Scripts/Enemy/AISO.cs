using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI")]
public class AISO : ScriptableObject
{
    [Header("===== Prefab =====")]
    public GameObject AIPrefab;

    [Header("===== Animation Controller =====")]
    public AnimatorOverrideController AIAnimator;

    [Header("===== HP =====")]
    public int maxHP;

    [Header("===== Type =====")]
    public AIType type;

    [Header("===== Speed =====")]
    public float walkSpeed;
    public float runSpeed;

    [Header("===== Skill =====")]
    public List<AISkill> Skills = new List<AISkill>();


}

[Serializable]
public class AISkill
{
    public SkillSO skill;
    public float skillRange;
}
