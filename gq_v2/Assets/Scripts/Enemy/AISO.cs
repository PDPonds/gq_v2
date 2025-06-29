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

    public AISkill RandomSkill()
    {
        AISkill skill = new AISkill();
        int index = UnityEngine.Random.Range(0, Skills.Count);
        skill.skill = Skills[index].skill;
        skill.skillRange = Skills[index].skillRange;
        skill.skillChangeDuration = Skills[index].skillChangeDuration;
        return skill;
    }

}

[Serializable]
public class AISkill
{
    public SkillSO skill;
    public float skillRange;
    public float skillChangeDuration;
}
