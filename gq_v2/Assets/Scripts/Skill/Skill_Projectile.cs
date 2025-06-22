using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Projectile")]
public class Skill_Projectile : SkillSO
{
    [Header("===== Move =====")]
    public float projectileSpeed;
    public float projectileDuration;
    [Header("===== Particle =====")]
    public GameObject hitPrefab;

    public Skill_Projectile()
    {
        Type = SkillType.Projectile;
    }
}
