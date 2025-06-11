using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Projectile")]
public class Skill_Projectile : SkillSO
{
    public float projectileSpeed;
    public float projectileDuration;

    public Skill_Projectile()
    {
        Type = SkillType.Projectile;
    }
}
