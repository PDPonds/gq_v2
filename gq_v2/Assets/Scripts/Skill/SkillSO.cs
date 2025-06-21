using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSO : ScriptableObject
{
    public SkillType Type;
    [HideInInspector] public int activateCount;
    [Header("===== Damage =====")]
    public int skillDamage;
    [Header("===== Delay =====")]
    public float skillDelay;
    [Header("===== Propertyu =====")]
    public List<Skill_Visual_Property> skill_Property = new List<Skill_Visual_Property>();
}

[Serializable]
public class Skill_Visual_Property
{
    [Header("===== Skill Particle =====")]
    public GameObject particle;
    public float initPrefabTime;
    [Header("===== Skill Animator Controller =====")]
    public AnimatorOverrideController animationController;
    public float skillDuration;
    [Header("===== Particle Position =====")]
    public InitSkillPosition intiPosition;
    [Header("===== Skill Force =====")]
    public float skillForceDuration;
    public float skillForce;
    [Header("===== Knock back Force =====")]
    public float knockbackForce;
    public float knockbackDuration;
}
