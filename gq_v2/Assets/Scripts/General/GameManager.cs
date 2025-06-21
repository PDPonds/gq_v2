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
    Idle, Chase, ChangeAttack, Attack, Death
}

public enum AIType
{
    Peaceful, Defensive, Aggressive
}

public class GameManager : Singleton<GameManager>
{

}
