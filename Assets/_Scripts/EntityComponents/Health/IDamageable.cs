using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(AttackInfo info);
}

public struct AttackInfo
{
    public float Damage;

    public AttackInfo(float damage)
    {
        this.Damage = damage;
    }
}
