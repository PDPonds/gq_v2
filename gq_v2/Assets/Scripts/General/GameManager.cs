using UnityEngine;

public enum SkillType
{
    Projectile, BuffPlayer, AOE_AroundPlayer, AOE_AroundMask
}

public enum InitSkillPosition
{
    Hand , Body
}

public class GameManager : Singleton<GameManager>
{
    [Header("===== Mouse =====")]
    public LayerMask mousePositionMask;
}
