using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/AI")]
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

    public float distanceBetweenPlayer;
    [Header("===== Skill =====")]
    public float attackDelay;
    public List<AISkill> Skills = new List<AISkill>();

    public AISkill RandomSkill()
    {
        return Skills[UnityEngine.Random.Range(0, Skills.Count)];
    }

}

