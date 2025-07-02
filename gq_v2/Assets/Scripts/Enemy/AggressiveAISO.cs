using UnityEngine;

[CreateAssetMenu(menuName = "AI/AI_Aggressive")]
public class AggressiveAISO : AISO
{
    public float radius;
    public float angle;
    public LayerMask playerMask;
    public LayerMask obstructionMask;

    public AggressiveAISO()
    {
        type = AIType.Aggressive;
    }


}
