using UnityEngine;

[CreateAssetMenu(menuName = "AISkill/Projectile")]
public class AISkill_Projectile : AISkill
{
    [Header("===== Projectile =====")]
    public float projectile_Speed;
    public float projectile_Duration;

    public AISkill_Projectile()
    {
        skillType = SkillType.Projectile;
    }
}
