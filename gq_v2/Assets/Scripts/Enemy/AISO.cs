using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI")]
public class AISO : ScriptableObject
{
    [Header("===== Prefab =====")]
    public GameObject AIPrefab;

    [Header("===== HP =====")]
    public int maxHP;

    [Header("===== Type =====")]
    public AIType type;

    [Header("===== Speed =====")]
    public float walkSpeed;
    public float runSpeed;

    [Header("===== Skill =====")]
    public List<SkillSO> Skills = new List<SkillSO>();


}
