using UnityEngine;

public enum SkillType
{
    Projectile, BuffUser, AOE_AroundUser, AOE_AroundMask
}

public enum aiBehavior
{
    Idle, Runaway, Chase, ChangeAttack, Attack, TakeDamage, Death
}

public enum AIType
{
    Peaceful, Defensive, Aggressive, Dummy
}

public class GameManager : Singleton<GameManager>
{
    
}
