using UnityEngine;

public enum SkillType
{
    Projectile, BuffPlayer, AOE_AroundPlayer, AOE_AroundMask
}

public enum InitSkillPosition
{
    Hand, Body
}

public enum aiBehavior
{
    Idle, Runaway, Chase, ChangeAttack, Attack, TakeDamage, Death
}

public enum AIType
{
    Peaceful, Defensive, Aggressive , Dummy
}

public class GameManager : Singleton<GameManager>
{

}
