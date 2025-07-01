using UnityEngine;

public class AISkill : ScriptableObject
{
    [Header("===== Delay =====")]
    public float skillDelay;

    [Header("===== Damage =====")]
    public int skillDamage;

    [Header("===== Range =====")]
    public float skillRange;

    [Header("===== Change =====")]
    public GameObject changeParticle;
    public Vector3 changeParticleOffset;
    public float changeDuration;
    public float changeSize;

    [Header("===== Attack =====")]
    public GameObject attackParticle;
    public Vector3 attackParticleOffset;
    public float attackDuration;

    [Header("===== Type =====")]
    public SkillType skillType;
}
