using UnityEngine;

public interface ICombatable
{
    public int curHp { get; set; }

    public void TakeDamage(int damage, float knockbackForce, float knockbackDuration);

    public void Heal(int amount);
    public void Death();
}
